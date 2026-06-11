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
    public class BookmarkRepository : Repository<Bookmark>, IBookmarkRepository
    {
        private const string UserPostUniqueIndex = "UX_Bookmarks_UserId_PostId";
        private readonly BlogAppDbContext _context;

        public BookmarkRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(Bookmark Bookmark, bool Created)> CreateIfNotExistsAsync(Bookmark bookmark, CancellationToken cancellationToken)
        {
            var existingBookmark = await _context.Bookmarks.FirstOrDefaultAsync(item => item.UserId == bookmark.UserId && item.PostId == bookmark.PostId, cancellationToken);
            if (existingBookmark != null)
            {
                if (existingBookmark.isActive && !existingBookmark.isDeleted)
                {
                    return (existingBookmark, false);
                }

                existingBookmark.isActive = true;
                existingBookmark.isDeleted = false;
                existingBookmark.CreatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                return (existingBookmark, true);
            }

            await _context.Bookmarks.AddAsync(bookmark, cancellationToken);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return (bookmark, true);
            }
            catch (DbUpdateException exception) when (IsDuplicateBookmark(exception))
            {
                _context.Entry(bookmark).State = EntityState.Detached;

                existingBookmark = await _context.Bookmarks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        item => item.UserId == bookmark.UserId && item.PostId == bookmark.PostId,
                        cancellationToken);

                if (existingBookmark != null)
                {
                    return (existingBookmark, false);
                }

                throw;
            }
        }

        public async Task<bool> DeleteByUserAndPostAsync(int userId, int postId, CancellationToken cancellationToken)
        {
            var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(item => item.UserId == userId && item.PostId == postId, cancellationToken);
            if (bookmark == null)
            {
                return false;
            }

            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<(List<BookmarkDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken)
        {
            var query = VisibleBookmarks(userId);
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(bookmark => bookmark.CreatedAt)
                .ThenByDescending(bookmark => bookmark.Id)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(bookmark => new BookmarkDto
                {
                    Id = bookmark.Id,
                    PostId = bookmark.PostId,
                    SavedAt = bookmark.CreatedAt,
                    Post = new PostDto
                    {
                        Id = bookmark.Post.Id,
                        Content = bookmark.Post.Content,
                        UserId = bookmark.Post.UserId,
                        Tags = bookmark.Post.Tags
                            .Select(tag => new TagDto
                            {
                                Id = tag.Id,
                                Name = tag.Name
                            })
                            .ToList(),
                        LikeCount = bookmark.Post.Likes.Count(like => like.isActive && !like.isDeleted),
                        CommentCount = bookmark.Post.Comments.Count(comment => comment.isActive && !comment.isDeleted),
                        BookmarkCount = bookmark.Post.Bookmarks.Count(item => item.isActive && !item.isDeleted)
                    }
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public Task<Bookmark?> GetByUserAndPostAsync(int userId, int postId, CancellationToken cancellationToken) =>
            _context.Bookmarks
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    bookmark => bookmark.UserId == userId &&
                                bookmark.PostId == postId &&
                                bookmark.isActive &&
                                !bookmark.isDeleted &&
                                bookmark.Post.Status == PostStatus.Published &&
                                bookmark.Post.isPublished &&
                                bookmark.Post.isActive &&
                                !bookmark.Post.isDeleted,
                    cancellationToken);

        private IQueryable<Bookmark> VisibleBookmarks(int userId) =>
            _context.Bookmarks.Where(bookmark =>
                bookmark.UserId == userId &&
                bookmark.isActive &&
                !bookmark.isDeleted &&
                bookmark.Post.Status == PostStatus.Published &&
                bookmark.Post.isPublished &&
                bookmark.Post.isActive &&
                !bookmark.Post.isDeleted);

        private static bool IsDuplicateBookmark(DbUpdateException exception) =>
            exception.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: UserPostUniqueIndex
            };
    }
}
