using buduns_server.Application.Dtos;
using buduns_server.Application.Repositories.Common;
using buduns_server.Domain.Entities;

namespace buduns_server.Application.Repositories
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
