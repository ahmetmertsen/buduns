using blogapp_server.Application.Dtos.Auth;

namespace blogapp_server.Application.Features.Auth.GetSessions
{
    public record GetAuthSessionsQueryResponse(IReadOnlyList<AuthSessionDto> Sessions);
}
