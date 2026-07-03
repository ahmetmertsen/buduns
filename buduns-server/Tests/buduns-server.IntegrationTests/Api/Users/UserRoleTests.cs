using buduns_server.Application.Abstractions.Services;
using buduns_server.Application.Common.Consts;
using buduns_server.Application.Exceptions;
using buduns_server.Application.Features.Users.Commands.AssignRoleToUser;
using buduns_server.IntegrationTests.Fixtures;
using buduns_server.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using User = buduns_server.Domain.Entities.Identity.User;

namespace buduns_server.IntegrationTests.Api.Users;

public sealed class UserRoleTests : IntegrationTestBase
{
    public UserRoleTests(BudunsWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Admin_should_not_demote_self()
    {
        var admin = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "self-admin", "Self Admin", RoleConstants.Admin));
        using var authentication = await Factory.CreateAuthenticatedClientAsync(admin.Id);
        var response = await authentication.Client.PostAsJsonAsync("/api/User/assignRoleToUser", new AssignRoleToUserCommand { TargetUserId = admin.Id, Roles = new[] { RoleConstants.User } });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Last_admin_role_should_not_be_removed()
    {
        var admin = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "last-admin", "Last Admin", RoleConstants.Admin));
        var action = async () => await Factory.ExecuteScopeAsync(async services => await services.GetRequiredService<IUserService>().AssignRoleToUserAsync(999999, admin.Id, new[] { RoleConstants.User }, CancellationToken.None));
        await action.Should().ThrowAsync<BadRequestException>().WithMessage("*en az bir Admin*");
    }

    [Fact]
    public async Task Empty_or_unknown_roles_should_be_rejected()
    {
        var admin = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "role-admin", "Role Admin", RoleConstants.Admin));
        var target = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "role-target", "Role Target", RoleConstants.User));
        using var authentication = await Factory.CreateAuthenticatedClientAsync(admin.Id);
        var emptyResponse = await authentication.Client.PostAsJsonAsync("/api/User/assignRoleToUser", new AssignRoleToUserCommand { TargetUserId = target.Id, Roles = Array.Empty<string>() });
        var unknownResponse = await authentication.Client.PostAsJsonAsync("/api/User/assignRoleToUser", new AssignRoleToUserCommand { TargetUserId = target.Id, Roles = new[] { "UnknownRole" } });
        emptyResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        unknownResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Admin_should_assign_existing_roles_to_another_user()
    {
        var firstAdmin = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "first-admin", "First Admin", RoleConstants.Admin));
        var target = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "new-moderator", "New Moderator", RoleConstants.User));
        using var authentication = await Factory.CreateAuthenticatedClientAsync(firstAdmin.Id);
        var response = await authentication.Client.PostAsJsonAsync("/api/User/assignRoleToUser", new AssignRoleToUserCommand { TargetUserId = target.Id, Roles = new[] { RoleConstants.User, RoleConstants.Moderator } });
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var roles = await Factory.ExecuteScopeAsync(async services => await services.GetRequiredService<UserManager<User>>().GetRolesAsync((await services.GetRequiredService<UserManager<User>>().FindByIdAsync(target.Id.ToString()))!));
        roles.Should().BeEquivalentTo(new[] { RoleConstants.User, RoleConstants.Moderator });
    }
}
