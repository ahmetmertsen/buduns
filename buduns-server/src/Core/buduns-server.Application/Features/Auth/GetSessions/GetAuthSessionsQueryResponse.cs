using buduns_server.Application.Dtos.Auth;

namespace buduns_server.Application.Features.Auth.GetSessions
{
    public record GetAuthSessionsQueryResponse(IReadOnlyList<AuthSessionDto> Sessions);
}
