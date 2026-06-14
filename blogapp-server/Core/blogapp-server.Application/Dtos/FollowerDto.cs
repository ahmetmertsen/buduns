namespace blogapp_server.Application.Dtos
{
    public class FollowerDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime FollowedAt { get; set; }
    }
}
