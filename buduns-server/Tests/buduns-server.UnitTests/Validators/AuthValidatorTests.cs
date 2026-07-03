using buduns_server.Application.Features.Auth.Login;
using buduns_server.Application.Features.Auth.RefreshTokenLogin;
using buduns_server.Application.Features.Auth.Register;

namespace buduns_server.UnitTests.Validators;

public class AuthValidatorTests
{
    [Fact]
    public async Task Login_ValidCredentials_ShouldSucceed()
    {
        var result = await new LoginUserCommandValidator().ValidateAsync(new LoginUserCommand("user@example.com", "secret"));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "secret", "UsernameOrEmail")]
    [InlineData("user", "", "Password")]
    public async Task Login_MissingCredential_ShouldFail(string usernameOrEmail, string password, string propertyName)
    {
        var result = await new LoginUserCommandValidator().ValidateAsync(new LoginUserCommand(usernameOrEmail, password));

        Assert.Contains(result.Errors, error => error.PropertyName == propertyName);
    }

    [Fact]
    public async Task Register_ValidRequest_ShouldSucceed()
    {
        var result = await new RegisterUserCommandValidator().ValidateAsync(
            new RegisterUserCommand("ahmet", "Ahmet Mert", "ahmet@example.com", "123456"));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("ab", "Ahmet Mert", "ahmet@example.com", "123456", "UserName")]
    [InlineData("ahmet", "", "ahmet@example.com", "123456", "FullName")]
    [InlineData("ahmet", "Ahmet Mert", "invalid-email", "123456", "Email")]
    [InlineData("ahmet", "Ahmet Mert", "ahmet@example.com", "12345", "Password")]
    public async Task Register_InvalidField_ShouldFail(string userName, string fullName, string email, string password, string propertyName)
    {
        var result = await new RegisterUserCommandValidator().ValidateAsync(
            new RegisterUserCommand(userName, fullName, email, password));

        Assert.Contains(result.Errors, error => error.PropertyName == propertyName);
    }

    [Fact]
    public async Task RefreshToken_EmptyToken_ShouldFail()
    {
        var result = await new RefreshTokenLoginCommandValidator().ValidateAsync(new RefreshTokenLoginCommand { RefreshToken = "" });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(RefreshTokenLoginCommand.RefreshToken));
    }

    [Fact]
    public async Task RefreshToken_NonEmptyToken_ShouldSucceed()
    {
        var result = await new RefreshTokenLoginCommandValidator().ValidateAsync(new RefreshTokenLoginCommand { RefreshToken = "refresh-token" });

        Assert.True(result.IsValid);
    }
}
