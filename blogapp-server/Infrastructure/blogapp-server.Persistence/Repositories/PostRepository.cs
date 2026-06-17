using blogapp_server.Application.Dtos;
using blogapp_server.Application.Repositories;
using blogapp_server.Application.Features.Posts.Queries.GetDailyTopPosts;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace blogapp_server.Persistence.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly BlogAppDbContext _context;

        public PostRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public override Task<List<Post>> GetAllAsync() =>
            VisiblePosts()
                .Include(post => post.Tags)
                .Include(post => post.Likes)
                .ThenInclude(like => like.User)
                .Include(post => post.Comments)
                .Include(post => post.Bookmarks)
                .ToListAsync();

        public override Task<Post?> GetByIdAsync(int id) =>
            VisiblePosts()
                .Include(post => post.Tags)
                .Include(post => post.Likes)
                .ThenInclude(like => like.User)
                .Include(post => post.Comments)
                .Include(post => post.Bookmarks)
                .FirstOrDefaultAsync(post => post.Id == id);

        public async Task<(List<Post> Items, int TotalCount)> GetPagedByTagIdAsync(int tagId, int page, int size, CancellationToken cancellationToken = default)
        {
            var query = VisiblePosts().Where(post => post.Tags.Any(tag => tag.Id == tagId && tag.isActive && !tag.isDeleted));
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Include(post => post.Tags).Include(post => post.Likes).ThenInclude(like => like.User).Include(post => post.Comments).Include(post => post.Bookmarks).OrderByDescending(post => post.CreatedAt).ThenByDescending(post => post.Id).Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public Task<Post?> GetByIdWithTagsAsync(int id) =>
            _context.Posts
                .Where(post =>
                    post.Status == PostStatus.Published &&
                    post.isActive &&
                    !post.isDeleted)
                .Include(post => post.Tags)
                .FirstOrDefaultAsync(post => post.Id == id);

        public Task<bool> ExistsVisibleAsync(int id, CancellationToken cancellationToken = default) => VisiblePosts().AnyAsync(post => post.Id == id, cancellationToken);

        public Task<int?> GetVisibleOwnerIdAsync(int id, CancellationToken cancellationToken = default) => VisiblePosts().Where(post => post.Id == id).Select(post => (int?)post.UserId).FirstOrDefaultAsync(cancellationToken);

        public async Task<List<TopPostDto>> GetDailyTopPostsAsync(DateTime startDateUtc, DateTime endDateUtc, int limit, CancellationToken cancellationToken = default)
        {
            var safeLimit = Math.Clamp(limit, 1, 100);

            return await VisiblePosts()
                .AsNoTracking()
                .Where(post => post.isPublished)
                .Select(post => new
                {
                    Post = post,
                    DailyLikeCount = post.Likes.Count(like => like.UserId != post.UserId && like.User.Status != UserStatus.Banned && like.CreatedAt >= startDateUtc && like.CreatedAt < endDateUtc && like.isActive && !like.isDeleted),
                    DailyCommentCount = post.Comments.Count(comment => comment.User.Status != UserStatus.Banned && comment.CreatedAt >= startDateUtc && comment.CreatedAt < endDateUtc && comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted),
                    LikeCount = post.Likes.Count(like => like.User.Status != UserStatus.Banned && like.isActive && !like.isDeleted),
                    CommentCount = post.Comments.Count(comment => comment.User.Status != UserStatus.Banned && comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted),
                    BookmarkCount = post.Bookmarks.Count(bookmark => bookmark.isActive && !bookmark.isDeleted)
                })
                .Where(item => item.DailyLikeCount > 0 || item.DailyCommentCount > 0)
                .OrderByDescending(item => (item.DailyLikeCount * 0.4) + (item.DailyCommentCount * 0.6))
                .ThenByDescending(item => item.DailyCommentCount)
                .ThenByDescending(item => item.DailyLikeCount)
                .ThenByDescending(item => item.Post.CreatedAt)
                .Take(safeLimit)
                .Select(item => new TopPostDto
                {
                    PostId = item.Post.Id,
                    Content = item.Post.Content,
                    UserId = item.Post.UserId,
                    DailyLikeCount = item.DailyLikeCount,
                    DailyCommentCount = item.DailyCommentCount,
                    LikeCount = item.LikeCount,
                    CommentCount = item.CommentCount,
                    BookmarkCount = item.BookmarkCount,
                    Score = (item.DailyLikeCount * 0.4) + (item.DailyCommentCount * 0.6)
                })
                .ToListAsync(cancellationToken);
        }

        private IQueryable<Post> VisiblePosts() => _context.Posts.Where(post => post.Status == PostStatus.Published && post.isPublished && post.isActive && !post.isDeleted);
    }
}
