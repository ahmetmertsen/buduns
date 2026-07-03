using buduns_server.Application.Dtos.Auth;

namespace buduns_server.Application.Abstractions.Services
{
    public interface IAuthSessionService
    {
        Task CreateSessionAsync(int userId, Guid sessionId, Guid tokenFamilyId, string refreshToken, DateTime expiresAt, CancellationToken cancellationToken);
        Task<AuthSessionRotationResult> RotateSessionAsync(string currentRefreshToken, Guid replacementSessionId, string replacementRefreshToken, DateTime replacementExpiresAt,CancellationToken cancellationToken);
        Task<bool> IsSessionActiveAsync(int userId, Guid sessionId, CancellationToken cancellationToken);
        Task<IReadOnlyList<AuthSessionDto>> GetActiveSessionsAsync(int userId, Guid currentSessionId, CancellationToken cancellationToken);
        Task<bool> RevokeSessionAsync(int userId, Guid sessionId, string reason, CancellationToken cancellationToken);
        Task RevokeAllSessionsAsync(int userId, string reason, CancellationToken cancellationToken);
    }
}
