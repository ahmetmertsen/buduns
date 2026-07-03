using buduns_server.Domain.Enums;

namespace buduns_server.Application.Dtos.User
{
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public UserStatus Status { get; set; }
        public DateTime? SuspendedUntil { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool IsLockedOut { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
