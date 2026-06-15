namespace blogapp_server.Application.Dtos
{
    public class LikedPostDto
    {
        public int LikeId { get; set; }
        public DateTime LikedAt { get; set; }
        public PostDto Post { get; set; } = new();
    }
}
