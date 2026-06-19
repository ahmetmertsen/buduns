using blogapp_server.Domain.Entities.Identity;

namespace blogapp_server.Domain.Entities
{
    public class AuthSession
    {
        public Guid Id { get; set; }
        public Guid TokenFamilyId { get; set; }
        public int UserId { get; set; }
        public string RefreshTokenHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedReason { get; set; }
        public Guid? ReplacedBySessionId { get; set; }
        public string? DeviceName { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }

        public User User { get; set; } = null!;
    }
}
