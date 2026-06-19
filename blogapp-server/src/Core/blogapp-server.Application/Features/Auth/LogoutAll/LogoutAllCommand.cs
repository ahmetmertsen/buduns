using blogapp_server.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Auth.LogoutAll
{
    public class LogoutAllCommand : IRequest<LogoutAllCommandResponse>, ICurrentUserRequest, IAllowUnverifiedEmail
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
