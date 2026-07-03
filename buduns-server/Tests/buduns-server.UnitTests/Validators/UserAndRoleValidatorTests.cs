using buduns_server.Application.Features.Roles.Commands.Create;
using buduns_server.Application.Features.Roles.Commands.Update;
using buduns_server.Application.Features.Users.Commands.AssignRoleToUser;
using buduns_server.Application.Features.Users.Commands.Update.UpdateEmail;
using buduns_server.Application.Features.Users.Commands.Update.UpdateMailVerify;
using buduns_server.Application.Features.Users.Commands.Update.UpdatePassword;
using buduns_server.Application.Features.Users.Queries.GetAll;
using buduns_server.Domain.Enums;

namespace buduns_server.UnitTests.Validators;

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

    [Fact]
    public async Task UpdatePassword_ValidCode_ShouldSucceed()
    {
        var result = await new UpdateUserPasswordCommandValidator().ValidateAsync(new UpdateUserPasswordCommand
        {
            EmailOrUsername = "ahmet@example.com",
            VerificationCode = "123456",
            newPassword = "123456",
            newPasswordConfirmed = "123456"
        });

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345")]
    [InlineData("abcdef")]
    public async Task UpdatePassword_InvalidCode_ShouldFail(string verificationCode)
    {
        var result = await new UpdateUserPasswordCommandValidator().ValidateAsync(new UpdateUserPasswordCommand
        {
            EmailOrUsername = "ahmet@example.com",
            VerificationCode = verificationCode,
            newPassword = "123456",
            newPasswordConfirmed = "123456"
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateUserPasswordCommand.VerificationCode));
    }

    [Fact]
    public async Task UpdateMailVerify_ValidCode_ShouldSucceed()
    {
        var result = await new UpdateUserMailVerifyCommandValidator().ValidateAsync(new UpdateUserMailVerifyCommand { VerificationCode = "123456" });

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task UpdateEmail_ValidCodes_ShouldSucceed()
    {
        var result = await new UpdateUserEmailCommandValidator().ValidateAsync(new UpdateUserEmailCommand
        {
            OldEmailVerificationCode = "111111",
            NewEmailVerificationCode = "222222",
            NewEmail = "new@example.com"
        });

        Assert.True(result.IsValid);
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
