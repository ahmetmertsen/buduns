using blogapp_server.Application.Features.Roles.Commands.Create;
using blogapp_server.Application.Features.Roles.Commands.Update;
using blogapp_server.Application.Features.Users.Commands.AssignRoleToUser;
using blogapp_server.Application.Features.Users.Queries.GetAll;
using blogapp_server.Domain.Enums;

namespace blogapp_server.UnitTests.Validators;

public class UserAndRoleValidatorTests
{
    [Fact]
    public async Task GetAllUsers_ValidRequest_ShouldSucceed()
    {
        var result = await new GetAllUsersQueryValidator().ValidateAsync(new GetAllUsersQuery
        {
            Page = 1,
            Size = 100,
            Search = "ahmet",
            Status = UserStatus.Active,
            EmailConfirmed = true
        });

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0, 20, "Page")]
    [InlineData(1, 0, "Size")]
    [InlineData(1, 101, "Size")]
    public async Task GetAllUsers_InvalidPagination_ShouldFail(int page, int size, string propertyName)
    {
        var result = await new GetAllUsersQueryValidator().ValidateAsync(new GetAllUsersQuery { Page = page, Size = size });

        Assert.Contains(result.Errors, error => error.PropertyName == propertyName);
    }

    [Fact]
    public async Task GetAllUsers_SearchLongerThanLimit_ShouldFail()
    {
        var result = await new GetAllUsersQueryValidator().ValidateAsync(new GetAllUsersQuery { Search = new string('a', 101) });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAllUsersQuery.Search));
    }

    [Fact]
    public async Task GetAllUsers_InvalidStatus_ShouldFail()
    {
        var result = await new GetAllUsersQueryValidator().ValidateAsync(new GetAllUsersQuery { Status = (UserStatus)999 });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAllUsersQuery.Status));
    }

    [Fact]
    public async Task AssignRole_ValidRequest_ShouldSucceed()
    {
        var result = await new AssignRoleToUserCommandValidator().ValidateAsync(new AssignRoleToUserCommand
        {
            TargetUserId = 10,
            Roles = new[] { "Admin", "User" }
        });

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task AssignRole_InvalidTargetUserId_ShouldFail()
    {
        var result = await new AssignRoleToUserCommandValidator().ValidateAsync(new AssignRoleToUserCommand
        {
            TargetUserId = 0,
            Roles = new[] { "User" }
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(AssignRoleToUserCommand.TargetUserId));
    }

    [Fact]
    public async Task AssignRole_EmptyRoles_ShouldFail()
    {
        var result = await new AssignRoleToUserCommandValidator().ValidateAsync(new AssignRoleToUserCommand
        {
            TargetUserId = 1,
            Roles = Array.Empty<string>()
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(AssignRoleToUserCommand.Roles));
    }

    [Fact]
    public async Task AssignRole_DuplicateRolesIgnoringCase_ShouldFail()
    {
        var result = await new AssignRoleToUserCommandValidator().ValidateAsync(new AssignRoleToUserCommand
        {
            TargetUserId = 1,
            Roles = new[] { "Admin", "admin" }
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(AssignRoleToUserCommand.Roles));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateRole_BlankName_ShouldFail(string name)
    {
        var result = await new CreateRoleCommandValidator().ValidateAsync(new CreateRoleCommand { Name = name });

        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task CreateRole_NameLongerThanLimit_ShouldFail()
    {
        var result = await new CreateRoleCommandValidator().ValidateAsync(new CreateRoleCommand { Name = new string('r', 101) });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateRoleCommand.Name));
    }

    [Fact]
    public async Task UpdateRole_ValidRequest_ShouldSucceed()
    {
        var result = await new UpdateRoleCommandValidator().ValidateAsync(new UpdateRoleCommand { Id = 1, Name = "Editor" });

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task UpdateRole_InvalidIdAndName_ShouldFailBothProperties()
    {
        var result = await new UpdateRoleCommandValidator().ValidateAsync(new UpdateRoleCommand { Id = 0, Name = " " });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateRoleCommand.Id));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateRoleCommand.Name));
    }
}
