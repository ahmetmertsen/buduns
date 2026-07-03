using buduns_server.Application.Abstractions.Services;
using buduns_server.Application.Abstractions.Token;
using buduns_server.Domain.Entities.Identity;
using buduns_server.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace buduns_server.IntegrationTests.Helpers;

public sealed record AuthenticatedClient(HttpClient Client, string AccessToken, string RefreshToken, Guid SessionId) : IDisposable
{
    public void Dispose() => Client.Dispose();
}

public static class AuthenticationHelper
{
    public static async Task<AuthenticatedClient> CreateAuthenticatedClientAsync(this BudunsWebApplicationFactory factory, int userId)
    {
        var authentication = await factory.ExecuteScopeAsync(async services =>
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var tokenHandler = services.GetRequiredService<ITokenHandler>();
            var sessionService = services.GetRequiredService<IAuthSessionService>();
            var user = await userManager.FindByIdAsync(userId.ToString()) ?? throw new InvalidOperationException($"Test user {userId} was not found.");
            var roles = await userManager.GetRolesAsync(user);
            var sessionId = Guid.NewGuid();
            var tokenFamilyId = Guid.NewGuid();
            var refreshToken = tokenHandler.CreateRefreshToken();
            await sessionService.CreateSessionAsync(user.Id, sessionId, tokenFamilyId, refreshToken, DateTime.UtcNow.AddDays(30), CancellationToken.None);
            return tokenHandler.CreateAccessToken(user, roles, sessionId, refreshToken);
        });
        var client = factory.CreateHttpsClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authentication.AccessToken);
        return new AuthenticatedClient(client, authentication.AccessToken, authentication.RefreshToken, authentication.SessionId);
    }
}
