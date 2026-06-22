using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Features.Auth.Login;
using blogapp_server.IntegrationTests.Fixtures;
using blogapp_server.IntegrationTests.Helpers;
using blogapp_server.WebAPI.Models;

namespace blogapp_server.IntegrationTests.Api;

public sealed class ApiContractTests : IntegrationTestBase
{
    public ApiContractTests(BlogAppWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Protected_endpoint_without_token_should_return_unauthorized()
    {
        using var client = Factory.CreateHttpsClient();
        var response = await client.PostAsync("/api/Like/1", null);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Protected_endpoint_without_permission_should_return_forbidden()
    {
        var user = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "regular-user", "Regular User", RoleConstants.User));
        using var authentication = await Factory.CreateAuthenticatedClientAsync(user.Id);
        var response = await authentication.Client.PostAsync("/api/Like/1", null);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Validation_error_should_follow_global_error_contract()
    {
        using var client = Factory.CreateHttpsClient();
        var response = await client.PostAsJsonAsync("/api/Auth/login", new LoginUserCommand(string.Empty, string.Empty));
        var body = await response.Content.ReadFromJsonAsync<ApiResponse>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body.Should().NotBeNull();
        body!.IsSuccess.Should().BeFalse();
        body.Error!.Code.Should().Be("VALIDATION_ERROR");
        body.Error.HttpStatus.Should().Be(400);
        body.Error.TraceId.Should().NotBeNullOrWhiteSpace();
        body.Error.ValidationErrors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Not_found_error_should_follow_global_error_contract()
    {
        using var client = Factory.CreateHttpsClient();
        var response = await client.GetAsync("/api/User/getUserById/999999");
        var body = await response.Content.ReadFromJsonAsync<ApiResponse>();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        body.Should().NotBeNull();
        body!.IsSuccess.Should().BeFalse();
        body.Error!.HttpStatus.Should().Be(404);
        body.Error.Code.Should().NotBeNullOrWhiteSpace();
    }
}
