using blogapp_server.Application.Dtos;
using blogapp_server.Application.Repositories;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace blogapp_server.Persistence.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private const string UserPostUniqueIndex = "UX_Likes_UserId_PostId";
        private static readonly TimeSpan NotificationCooldown = TimeSpan.FromHours(1);
        private readonly BlogAppDbContext _context;

        public LikeRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(Like Like, bool Created)> CreateIfNotExistsAsync(Like like, Notification? notification, CancellationToken cancellationToken)
        {
            var existingLike = await _context.Likes.FirstOrDefaultAsync(item => item.UserId == like.UserId && item.PostId == like.PostId, cancellationToken);
            if (existingLike != null)
            {
                if (existingLike.isActive && !existingLike.isDeleted)
                {
                    return (existingLike, false);
                }

                existingLike.isActive = true;
                existingLike.isDeleted = false;
                existingLike.CreatedAt = DateTime.UtcNow;
                await AddNotificationIfAllowedAsync(notification, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return (existingLike, true);
            }

            await _context.Likes.AddAsync(like, cancellationToken);
            await AddNotificationIfAllowedAsync(notification, cancellationToken);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return (like, true);
            }
            catch (DbUpdateException exception) when (IsDuplicateLike(exception))
            {
                _context.Entry(like).State = EntityState.Detached;
                DetachAddedNotification(notification);

                existingLike = await _context.Likes.AsNoTracking().FirstOrDefaultAsync(item => item.UserId == like.UserId && item.PostId == like.PostId, cancellationToken);
                if (existingLike != null)
                {
                    return (existingLike, false);
                }

                throw;
            }
        }

        public async Task<bool> DeleteByUserAndPostAsync(int userId, int postId, CancellationToken cancellationToken)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(item => item.UserId == userId && item.PostId == postId, cancellationToken);
            if (like == null)
            {
                return false;
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<Like?> GetByUserAndPostAsync(int userId, int postId, CancellationToken cancellationToken) => VisibleLikes().AsNoTracking().FirstOrDefaultAsync(like => like.UserId == userId && like.PostId == postId, cancellationToken);

        public async Task<(List<LikeDto> Items, int TotalCount)> GetPagedByPostIdAsync(int postId, int page, int size, CancellationToken cancellationToken)
        {
            var query = VisibleLikes().Where(like => like.PostId == postId && like.User.Status != UserStatus.Banned);
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(like => like.CreatedAt).ThenByDescending(like => like.Id).Skip((page - 1) * size).Take(size).AsNoTracking().Select(like => new LikeDto
            {
                Id = like.Id,
                UserId = like.UserId,
                UserName = like.User.UserName!,
                FullName = like.User.FullName,
                ImageUrl = like.User.ImageUrl,
                LikedAt = like.CreatedAt
            }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<LikedPostDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken)
        {
            var query = VisibleLikes().Where(like => like.UserId == userId);
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(like => like.CreatedAt).ThenByDescending(like => like.Id).Skip((page - 1) * size).Take(size).AsNoTracking().Select(like => new LikedPostDto
            {
                LikeId = like.Id,
                LikedAt = like.CreatedAt,
                Post = new PostDto
                {
                    Id = like.Post.Id,
                    Content = like.Post.Content,
                    UserId = like.Post.UserId,
                    Tags = like.Post.Tags.Select(tag => new TagDto { Id = tag.Id, Name = tag.Name }).ToList(),
                    LikeCount = like.Post.Likes.Count(item => item.isActive && !item.isDeleted && item.User.Status != UserStatus.Banned),
                    CommentCount = like.Post.Comments.Count(comment => comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted),
                    BookmarkCount = like.Post.Bookmarks.Count(bookmark => bookmark.isActive && !bookmark.isDeleted)
                }
            }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        private IQueryable<Like> VisibleLikes() => _context.Likes.Where(like => like.isActive && !like.isDeleted && like.Post.Status == PostStatus.Published && like.Post.isPublished && like.Post.isActive && !like.Post.isDeleted);

        private async Task AddNotificationIfAllowedAsync(Notification? notification, CancellationToken cancellationToken)
        {
            if (notification == null)
            {
                return;
            }

            var cooldownStart = DateTime.UtcNow.Subtract(NotificationCooldown);
            var notificationExists = await _context.Notifications.AnyAsync(item => item.Type == NotificationType.POST_LIKED && item.UserId == notification.UserId && item.ActorUserId == notification.ActorUserId && item.PostId == notification.PostId && item.CreatedAt >= cooldownStart, cancellationToken);
            if (!notificationExists)
            {
                await _context.Notifications.AddAsync(notification, cancellationToken);
            }
        }

        private void DetachAddedNotification(Notification? notification)
        {
            if (notification != null && _context.Entry(notification).State == EntityState.Added)
            {
                _context.Entry(notification).State = EntityState.Detached;
            }
        }

        private static bool IsDuplicateLike(DbUpdateException exception) => exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation, ConstraintName: UserPostUniqueIndex };
    }
}
