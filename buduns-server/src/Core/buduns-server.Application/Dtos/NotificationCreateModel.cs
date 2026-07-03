using buduns_server.Domain.Entities;
using buduns_server.Domain.Enums;

namespace buduns_server.Application.Dtos
{
    public class NotificationCreateModel
    {
        public NotificationType Type { get; set; }
        public int UserId { get; set; }
        public int? ActorUserId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
        public string? Message { get; set; }
        public TimeSpan? Cooldown { get; set; }
    }
}
