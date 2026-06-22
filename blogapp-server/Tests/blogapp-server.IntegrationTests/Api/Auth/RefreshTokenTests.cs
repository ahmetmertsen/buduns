using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Features.Auth.RefreshTokenLogin;
using blogapp_server.IntegrationTests.Fixtures;
using blogapp_server.IntegrationTests.Helpers;
using blogapp_server.Persistence.Context;
using blogapp_server.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace blogapp_server.IntegrationTests.Api.Auth;

public sealed class RefreshTokenTests : IntegrationTestBase
{
    public RefreshTokenTests(BlogAppWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Refresh_token_should_rotate_and_reuse_should_revoke_token_family()
    {
        var user = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "refresh-user", "Refresh User", RoleConstants.User));
        using var authentication = await Factory.CreateAuthenticatedClientAsync(user.Id);
        using var client = Factory.CreateHttpsClient();

        var firstResponse = await client.PostAsJsonAsync("/api/Auth/refreshTokenLogin", new RefreshTokenLoginCommand { RefreshToken = authentication.RefreshToken });
        var firstBody = await firstResponse.Content.ReadFromJsonAsync<RefreshTokenLoginCommandResponse>();
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        firstBody!.Token.RefreshToken.Should().NotBe(authentication.RefreshToken);

        var reuseResponse = await client.PostAsJsonAsync("/api/Auth/refreshTokenLogin", new RefreshTokenLoginCommand { RefreshToken = authentication.RefreshToken });
        var reuseBody = await reuseResponse.Content.ReadFromJsonAsync<ApiResponse>();
        reuseResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        reuseBody!.Error!.Code.Should().Be("INVALID_REFRESH_TOKEN");

        var activeSessionCount = await Factory.ExecuteScopeAsync(async services => await services.GetRequiredService<BlogAppDbContext>().AuthSessions.CountAsync(session => session.UserId == user.Id && session.RevokedAt == null));
        activeSessionCount.Should().Be(0);
    }
}
