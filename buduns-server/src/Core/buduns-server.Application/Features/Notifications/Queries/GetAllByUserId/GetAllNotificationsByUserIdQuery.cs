using buduns_server.Application.Common.Interfaces;
using buduns_server.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Notifications.Queries.GetAllByUserId
{
    public class GetAllNotificationsByUserIdQuery : IRequest<PagedResponse<NotificationDto>>, ICurrentUserRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
        public bool OnlyUnread { get; set; }
    }
}
