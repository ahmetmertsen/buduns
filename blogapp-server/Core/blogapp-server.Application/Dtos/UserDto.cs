namespace blogapp_server.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public bool IsFullNameVisible { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
