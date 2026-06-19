using blogapp_server.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Auth.Logout
{
    public class LogoutCommand : IRequest<LogoutCommandResponse>, ICurrentUserRequest, ICurrentSessionRequest, IAllowUnverifiedEmail
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public Guid CurrentSessionId { get; set; }
    }
}
