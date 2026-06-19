using blogapp_server.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Auth.GetSessions
{
    public class GetAuthSessionsQuery : IRequest<GetAuthSessionsQueryResponse>, ICurrentUserRequest, ICurrentSessionRequest, IAllowUnverifiedEmail
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public Guid CurrentSessionId { get; set; }
    }
}
