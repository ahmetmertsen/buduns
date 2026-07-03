using buduns_server.Application.Dtos;
using buduns_server.Application.Repositories.Common;
using buduns_server.Domain.Entities;

namespace buduns_server.Application.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<(List<PostDto> Items, int TotalCount)> GetPagedAsync(int page, int size, int? tagId, int? userId, string? search, string? sortBy, int? viewerUserId, CancellationToken cancellationToken = default);
        Task<(List<PostDto> Items, int TotalCount)> GetPagedByTagIdAsync(int tagId, int page, int size, int? viewerUserId, CancellationToken cancellationToken = default);
        Task<(List<PostDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, int? viewerUserId, CancellationToken cancellationToken = default);
        Task<(List<PostDto> Items, int TotalCount)> GetPagedFollowingAsync(int userId, int page, int size, CancellationToken cancellationToken = default);
        Task<PostDto?> GetDtoByIdAsync(int id, int? viewerUserId, CancellationToken cancellationToken = default);
        Task<Post?> GetByIdWithTagsAsync(int id);
        Task<bool> ExistsVisibleAsync(int id, CancellationToken cancellationToken = default);
        Task<int?> GetVisibleOwnerIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TopPostDto>> GetDailyTopPostsAsync(DateTime startDateUtc, DateTime endDateUtc, int limit, CancellationToken cancellationToken = default);
    }
}
