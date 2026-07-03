namespace buduns_server.Application.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? UserFullName { get; set; }
        public string? UserImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<TagDto> Tags { get; set; } = new List<TagDto>();
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int BookmarkCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsOwner { get; set; }
        public bool IsFollowingAuthor { get; set; }
    }
}
