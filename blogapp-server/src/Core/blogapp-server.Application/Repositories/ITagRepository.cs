using blogapp_server.Application.Dtos;
using blogapp_server.Application.Repositories.Common;
using blogapp_server.Domain.Entities;

namespace blogapp_server.Application.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<List<Tag>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken = default);
        Task<(List<TagDto> Items, int TotalCount)> GetPagedAsync(int page, int size, string? search, CancellationToken cancellationToken = default);
        Task<TagDto?> GetDtoByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Tag?> GetVisibleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNormalizedNameAsync(string normalizedName, int? excludeId = null, CancellationToken cancellationToken = default);
    }
}
