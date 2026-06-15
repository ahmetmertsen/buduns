using blogapp_server.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Notifications.Commands.MarkAsRead
{
    public class MarkNotificationAsReadCommand : IRequest<MarkNotificationAsReadCommandResponse>, ICurrentUserRequest
    {
        public int Id { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
