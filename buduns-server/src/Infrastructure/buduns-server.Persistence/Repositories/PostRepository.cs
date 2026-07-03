using buduns_server.Application.Dtos;
using buduns_server.Application.Repositories;
using buduns_server.Domain.Entities;
using buduns_server.Domain.Enums;
using buduns_server.Persistence.Context;
using buduns_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace buduns_server.Persistence.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly BudunsDbContext _context;

        public PostRepository(BudunsDbContext context) : base(context)
        {
            _context = context;
        }

        public override Task<List<Post>> GetAllAsync() => VisiblePosts().Include(post => post.Tags).Include(post => post.Likes).ThenInclude(like => like.User).Include(post => post.Comments).Include(post => post.Bookmarks).OrderByDescending(post => post.CreatedAt).ThenByDescending(post => post.Id).AsNoTracking().ToListAsync();

        public override Task<Post?> GetByIdAsync(int id) => VisiblePosts().Include(post => post.User).Include(post => post.Tags).Include(post => post.Likes).ThenInclude(like => like.User).Include(post => post.Comments).Include(post => post.Bookmarks).AsNoTracking().FirstOrDefaultAsync(post => post.Id == id);

        public async Task<(List<PostDto> Items, int TotalCount)> GetPagedAsync(int page, int size, int? tagId, int? userId, string? search, string? sortBy, int? viewerUserId, CancellationToken cancellationToken = default)
        {
            var query = VisiblePosts();
            if (tagId.HasValue)
            {
                query = query.Where(post => post.Tags.Any(tag => tag.Id == tagId.Value && tag.isActive && !tag.isDeleted));
            }

            if (userId.HasValue)
            {
                query = query.Where(post => post.UserId == userId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var keyword = search.Trim();
                query = query.Where(post => EF.Functions.ILike(post.Content, $"%{keyword}%"));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await ProjectToPostDto(OrderPosts(query, sortBy).Skip((page - 1) * size).Take(size), viewerUserId).ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public async Task<(List<PostDto> Items, int TotalCount)> GetPagedByTagIdAsync(int tagId, int page, int size, int? viewerUserId, CancellationToken cancellationToken = default)
        {
            var query = VisiblePosts().Where(post => post.Tags.Any(tag => tag.Id == tagId && tag.isActive && !tag.isDeleted));
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await ProjectToPostDto(OrderPosts(query, "recent").Skip((page - 1) * size).Take(size), viewerUserId).ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public async Task<(List<PostDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, int? viewerUserId, CancellationToken cancellationToken = default)
        {
            var query = VisiblePosts().Where(post => post.UserId == userId);
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await ProjectToPostDto(OrderPosts(query, "recent").Skip((page - 1) * size).Take(size), viewerUserId).ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public async Task<(List<PostDto> Items, int TotalCount)> GetPagedFollowingAsync(int userId, int page, int size, CancellationToken cancellationToken = default)
        {
            var query = VisiblePosts().Where(post => post.User.Followers.Any(follow => follow.FollowerId == userId && follow.isActive && !follow.isDeleted));
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await ProjectToPostDto(OrderPosts(query, "recent").Skip((page - 1) * size).Take(size), userId).ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public Task<PostDto?> GetDtoByIdAsync(int id, int? viewerUserId, CancellationToken cancellationToken = default) => ProjectToPostDto(VisiblePosts().Where(post => post.Id == id), viewerUserId).FirstOrDefaultAsync(cancellationToken);

        public Task<Post?> GetByIdWithTagsAsync(int id) => VisiblePosts().Include(post => post.Tags).FirstOrDefaultAsync(post => post.Id == id);

        public Task<bool> ExistsVisibleAsync(int id, CancellationToken cancellationToken = default) => VisiblePosts().AnyAsync(post => post.Id == id, cancellationToken);

        public Task<int?> GetVisibleOwnerIdAsync(int id, CancellationToken cancellationToken = default) => VisiblePosts().Where(post => post.Id == id).Select(post => (int?)post.UserId).FirstOrDefaultAsync(cancellationToken);

        public async Task<List<TopPostDto>> GetDailyTopPostsAsync(DateTime startDateUtc, DateTime endDateUtc, int limit, CancellationToken cancellationToken = default)
        {
            var safeLimit = Math.Clamp(limit, 1, 100);

            return await VisiblePosts().AsNoTracking().Select(post => new
            {
                Post = post,
                DailyLikeCount = post.Likes.Count(like => like.UserId != post.UserId && like.User.Status != UserStatus.Banned && like.CreatedAt >= startDateUtc && like.CreatedAt < endDateUtc && like.isActive && !like.isDeleted),
                DailyCommentCount = post.Comments.Count(comment => comment.User.Status != UserStatus.Banned && comment.CreatedAt >= startDateUtc && comment.CreatedAt < endDateUtc && comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted),
                LikeCount = post.Likes.Count(like => like.User.Status != UserStatus.Banned && like.isActive && !like.isDeleted),
                CommentCount = post.Comments.Count(comment => comment.User.Status != UserStatus.Banned && comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted),
                BookmarkCount = post.Bookmarks.Count(bookmark => bookmark.isActive && !bookmark.isDeleted)
            }).Where(item => item.DailyLikeCount > 0 || item.DailyCommentCount > 0).OrderByDescending(item => (item.DailyLikeCount * 0.4) + (item.DailyCommentCount * 0.6)).ThenByDescending(item => item.DailyCommentCount).ThenByDescending(item => item.DailyLikeCount).ThenByDescending(item => item.Post.CreatedAt).Take(safeLimit).Select(item => new TopPostDto
            {
                PostId = item.Post.Id,
                Content = item.Post.Content,
                UserId = item.Post.UserId,
                UserName = item.Post.User.UserName ?? string.Empty,
                UserFullName = item.Post.User.FullName,
                UserImageUrl = item.Post.User.ImageUrl,
                DailyLikeCount = item.DailyLikeCount,
                DailyCommentCount = item.DailyCommentCount,
                LikeCount = item.LikeCount,
                CommentCount = item.CommentCount,
                BookmarkCount = item.BookmarkCount,
                Score = (item.DailyLikeCount * 0.4) + (item.DailyCommentCount * 0.6)
            }).ToListAsync(cancellationToken);
        }

        private IQueryable<PostDto> ProjectToPostDto(IQueryable<Post> query, int? viewerUserId)
        {
            var hasViewer = viewerUserId.HasValue;
            var viewerId = viewerUserId ?? 0;

            return query.AsNoTracking().Select(post => new PostDto
            {
                Id = post.Id,
                Content = post.Content,
                UserId = post.UserId,
                UserName = post.User.UserName ?? string.Empty,
                UserFullName = post.User.FullName,
                UserImageUrl = post.User.ImageUrl,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdateAt,
                Tags = post.Tags.Where(tag => tag.isActive && !tag.isDeleted).OrderBy(tag => tag.Name).Select(tag => new TagDto { Id = tag.Id, Name = tag.Name }).ToList(),
                LikeCount = post.Likes.Count(like => like.isActive && !like.isDeleted && like.User.Status != UserStatus.Banned),
                CommentCount = post.Comments.Count(comment => comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted && comment.User.Status != UserStatus.Banned),
                BookmarkCount = post.Bookmarks.Count(bookmark => bookmark.isActive && !bookmark.isDeleted),
                IsLiked = hasViewer && post.Likes.Any(like => like.UserId == viewerId && like.isActive && !like.isDeleted),
                IsBookmarked = hasViewer && post.Bookmarks.Any(bookmark => bookmark.UserId == viewerId && bookmark.isActive && !bookmark.isDeleted),
                IsOwner = hasViewer && post.UserId == viewerId,
                IsFollowingAuthor = hasViewer && post.User.Followers.Any(follow => follow.FollowerId == viewerId && follow.isActive && !follow.isDeleted)
            });
        }

        private IQueryable<Post> OrderPosts(IQueryable<Post> query, string? sortBy) => sortBy?.Trim().ToLowerInvariant() switch
        {
            "oldest" => query.OrderBy(post => post.CreatedAt).ThenBy(post => post.Id),
            "popular" => query.OrderByDescending(post => post.Likes.Count(like => like.isActive && !like.isDeleted && like.User.Status != UserStatus.Banned) + post.Comments.Count(comment => comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted && comment.User.Status != UserStatus.Banned)).ThenByDescending(post => post.CreatedAt).ThenByDescending(post => post.Id),
            _ => query.OrderByDescending(post => post.CreatedAt).ThenByDescending(post => post.Id)
        };

        private IQueryable<Post> VisiblePosts() => _context.Posts.Where(post => post.Status == PostStatus.Published && post.isPublished && post.isActive && !post.isDeleted && post.User.Status != UserStatus.Banned);
    }
}
