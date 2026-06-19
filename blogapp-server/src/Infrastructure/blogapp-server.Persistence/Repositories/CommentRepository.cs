using blogapp_server.Application.Dtos;
using blogapp_server.Application.Repositories;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace blogapp_server.Persistence.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly BlogAppDbContext _context;

        public CommentRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Comment?> GetVisibleByIdAsync(int id, CancellationToken cancellationToken = default) => VisibleComments().Include(comment => comment.User).AsNoTracking().FirstOrDefaultAsync(comment => comment.Id == id, cancellationToken);

        public Task<Comment?> GetForMutationAsync(int id, CancellationToken cancellationToken = default) => _context.Comments.Include(comment => comment.Post).FirstOrDefaultAsync(comment => comment.Id == id, cancellationToken);

        public Task<CommentDto?> GetDtoByIdAsync(int id, CancellationToken cancellationToken = default) => ProjectToDto(VisibleComments().Where(comment => comment.Id == id)).FirstOrDefaultAsync(cancellationToken);

        public async Task<(List<CommentDto> Items, int TotalCount)> GetPagedByPostIdAsync(int postId, int page, int size, CancellationToken cancellationToken = default)
        {
            var query = VisibleComments().Where(comment => comment.PostId == postId);
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await ProjectToDto(query.OrderBy(comment => comment.CreatedAt).ThenBy(comment => comment.Id).Skip((page - 1) * size).Take(size)).ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public async Task<(List<CommentDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken = default)
        {
            var query = VisibleComments().Where(comment => comment.UserId == userId);
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await ProjectToDto(query.OrderByDescending(comment => comment.CreatedAt).ThenByDescending(comment => comment.Id).Skip((page - 1) * size).Take(size)).ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public Task<bool> HasRecentDuplicateAsync(int userId, int postId, string content, DateTime since, CancellationToken cancellationToken = default) => _context.Comments.AnyAsync(comment => comment.UserId == userId && comment.PostId == postId && comment.Content == content && comment.CreatedAt >= since && comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted, cancellationToken);

        public Task<int> CountRecentByUserAsync(int userId, DateTime since, CancellationToken cancellationToken = default) => _context.Comments.CountAsync(comment => comment.UserId == userId && comment.CreatedAt >= since, cancellationToken);

        private IQueryable<Comment> VisibleComments() => _context.Comments.Where(comment => comment.Status == CommentStatus.Published && comment.isActive && !comment.isDeleted && comment.Post.Status == PostStatus.Published && comment.Post.isPublished && comment.Post.isActive && !comment.Post.isDeleted);

        private static IQueryable<CommentDto> ProjectToDto(IQueryable<Comment> query) => query.AsNoTracking().Select(comment => new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            UserId = comment.UserId,
            UserName = comment.User.UserName,
            UserImageUrl = comment.User.ImageUrl,
            PostId = comment.PostId,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdateAt == default ? null : comment.UpdateAt,
            IsEdited = comment.UpdateAt != default
        });
    }
}
