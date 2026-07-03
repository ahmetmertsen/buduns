using buduns_server.Domain.Entities.Common;
using buduns_server.Domain.Entities.Identity;
using buduns_server.Domain.Enums;

namespace buduns_server.Domain.Entities
{
    public class VerificationChallenge : BaseEntity
    {
        public int UserId { get; set; }
        public VerificationPurpose Purpose { get; set; }
        public string? TargetEmail { get; set; }
        public string CodeHash { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public int AttemptCount { get; set; }
        public int MaxAttempts { get; set; }
        public DateTime? ConsumedAt { get; set; }
        public DateTime LastSentAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
    }
}
