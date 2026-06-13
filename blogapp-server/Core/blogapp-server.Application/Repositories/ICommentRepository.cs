using blogapp_server.Application.Dtos;
using blogapp_server.Application.Repositories.Common;
using blogapp_server.Domain.Entities;

namespace blogapp_server.Application.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment?> GetVisibleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Comment?> GetForMutationAsync(int id, CancellationToken cancellationToken = default);
        Task<CommentDto?> GetDtoByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<(List<CommentDto> Items, int TotalCount)> GetPagedByPostIdAsync(int postId, int page, int size, CancellationToken cancellationToken = default);
        Task<(List<CommentDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken = default);
        Task<bool> HasRecentDuplicateAsync(int userId, int postId, string content, DateTime since, CancellationToken cancellationToken = default);
        Task<int> CountRecentByUserAsync(int userId, DateTime since, CancellationToken cancellationToken = default);
    }
}
