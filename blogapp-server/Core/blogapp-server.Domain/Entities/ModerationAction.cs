using blogapp_server.Domain.Entities.Common;
using blogapp_server.Domain.Entities.Identity;
using blogapp_server.Domain.Enums;

namespace blogapp_server.Domain.Entities
{
    public class ModerationAction : BaseEntity
    {
        public int ReportId { get; set; }
        public int ModeratorUserId { get; set; }
        public ModerationActionType ActionType { get; set; }
        public ReportTargetType TargetType { get; set; }
        public int? TargetPostId { get; set; }
        public int? TargetUserId { get; set; }
        public string? Note { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public Report Report { get; set; } = null!;
        public User ModeratorUser { get; set; } = null!;
        public Post? TargetPost { get; set; }
        public User? TargetUser { get; set; }
    }
}
