using System.Reflection;
using System.Security.Claims;
using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Common.CustomAttrributes;
using blogapp_server.Domain.Enums;
using blogapp_server.WebAPI.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace blogapp_server.UnitTests.Filters;

public class RolePermissionFilterTests
{
    [Fact]
    public async Task Filter_ActionWithoutAuthorizeDefinition_ShouldContinue()
    {
        var userService = Substitute.For<IUserService>();
        var filter = CreateFilter(userService);
        var context = CreateContext(nameof(TestController.PublicAction), CreatePrincipal(1));
        var nextCalled = false;

        await filter.OnActionExecutionAsync(context, CreateNext(context, () => nextCalled = true));

        Assert.True(nextCalled);
        Assert.Null(context.Result);
    }

    [Fact]
    public async Task Filter_AdminUser_ShouldBypassPermissionService()
    {
        var userService = Substitute.For<IUserService>();
        var filter = CreateFilter(userService);
        var context = CreateContext(nameof(TestController.ProtectedAction), CreatePrincipal(1, "Admin"));
        var nextCalled = false;

        await filter.OnActionExecutionAsync(context, CreateNext(context, () => nextCalled = true));

        Assert.True(nextCalled);
        await userService.DidNotReceiveWithAnyArgs().HasRolePermissionToEndpointAsync(default, default!);
    }

    [Fact]
    public async Task Filter_InvalidUserIdentifier_ShouldReturn401()
    {
        var userService = Substitute.For<IUserService>();
        var filter = CreateFilter(userService);
        var context = CreateContext(nameof(TestController.ProtectedAction), CreatePrincipal(null));

        await filter.OnActionExecutionAsync(context, CreateNext(context));

        Assert.IsType<UnauthorizedResult>(context.Result);
    }

    [Fact]
    public async Task Filter_UserWithoutPermission_ShouldReturn403()
    {
        var userService = Substitute.For<IUserService>();
        userService.HasRolePermissionToEndpointAsync(7, "POST.Writing.CreatePost").Returns(false);
        var filter = CreateFilter(userService);
        var context = CreateContext(nameof(TestController.ProtectedAction), CreatePrincipal(7));

        await filter.OnActionExecutionAsync(context, CreateNext(context));

        Assert.IsType<ForbidResult>(context.Result);
    }

    [Fact]
    public async Task Filter_UserWithPermission_ShouldContinueAndUseExpectedPermissionCode()
    {
        var userService = Substitute.For<IUserService>();
        userService.HasRolePermissionToEndpointAsync(7, "POST.Writing.CreatePost").Returns(true);
        var filter = CreateFilter(userService);
        var context = CreateContext(nameof(TestController.ProtectedAction), CreatePrincipal(7));
        var nextCalled = false;

        await filter.OnActionExecutionAsync(context, CreateNext(context, () => nextCalled = true));

        Assert.True(nextCalled);
        await userService.Received(1).HasRolePermissionToEndpointAsync(7, "POST.Writing.CreatePost");
    }

    private static RolePermissionFilter CreateFilter(IUserService userService) =>
        new(userService, NullLogger<RolePermissionFilter>.Instance);

    private static ActionExecutingContext CreateContext(string methodName, ClaimsPrincipal user)
    {
        var method = typeof(TestController).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)!;
        var descriptor = new ControllerActionDescriptor
        {
            MethodInfo = method,
            ControllerName = nameof(TestController),
            ActionName = methodName
        };
        var httpContext = new DefaultHttpContext { User = user };
        var actionContext = new ActionContext(httpContext, new RouteData(), descriptor, new ModelStateDictionary());
        return new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object?>(), new TestController());
    }

    private static ActionExecutionDelegate CreateNext(ActionExecutingContext context, Action? callback = null) => () =>
    {
        callback?.Invoke();
        return Task.FromResult(new ActionExecutedContext(context, new List<IFilterMetadata>(), context.Controller));
    };

    private static ClaimsPrincipal CreatePrincipal(int? userId, string? role = null)
    {
        var claims = new List<Claim>();
        if (userId.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));
        }
        if (role != null)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));
    }

    private sealed class TestController
    {
        public void PublicAction()
        {
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = "Posts", ActionType = ActionType.Writing, Definition = "Create Post")]
        public void ProtectedAction()
        {
        }
    }
}
