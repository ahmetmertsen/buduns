using buduns_server.Domain.Enums;

namespace buduns_server.Application.Dtos
{
    public class ModerationActionDto
    {
        public int Id { get; set; }
        public ModerationActionType ActionType { get; set; }
        public int ModeratorUserId { get; set; }
        public string? ModeratorUserName { get; set; }
        public string? Note { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
