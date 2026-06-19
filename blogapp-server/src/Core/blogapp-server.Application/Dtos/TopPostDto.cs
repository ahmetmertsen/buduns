namespace blogapp_server.Application.Dtos
{
    public class TopPostDto
    {
        public int Rank { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? UserFullName { get; set; }
        public string? UserImageUrl { get; set; }
        public int DailyLikeCount { get; set; }
        public int DailyCommentCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int BookmarkCount { get; set; }
        public double Score { get; set; }
    }
}
