using blogapp_server.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadNotificationCountQuery : IRequest<GetUnreadNotificationCountQueryResponse>, ICurrentUserRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
