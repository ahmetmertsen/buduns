using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Common.CustomAttrributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace blogapp_server.WebAPI.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor == null)
            {
                await next();
                return;
            }

            var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
            if (attribute == null)
            {
                await next();
                return;
            }

            if (context.HttpContext.User.IsInRole("Admin"))
            {
                await next();
                return;
            }

            var name = context.HttpContext.User.Identity?.Name
                ?? context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value
                ?? context.HttpContext.User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
            if (string.IsNullOrEmpty(name))
            {
                context.Result = new UnauthorizedResult(); // 401: login yok
                return;
            }



            var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

            var httpType = httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get;

            var code = $"{httpType}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

            var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);

            if (!hasRole)
            {
                context.Result = new ForbidResult(); // 403: login var ama yetki yok
                return;
            }

            await next();
        }
    }
}
