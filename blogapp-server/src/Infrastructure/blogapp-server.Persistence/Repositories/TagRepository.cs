using blogapp_server.Application.Common.Helpers;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.Repositories;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace blogapp_server.Persistence.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly BlogAppDbContext _context;

        public TagRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public override Task<List<Tag>> GetAllAsync() => VisibleTags().AsNoTracking().OrderBy(tag => tag.Name).ToListAsync();

        public Task<List<Tag>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken = default)
        {
            var distinctIds = ids.Distinct().ToList();
            return VisibleTags().Where(tag => distinctIds.Contains(tag.Id)).ToListAsync(cancellationToken);
        }

        public async Task<(List<TagDto> Items, int TotalCount)> GetPagedAsync(int page, int size, string? search, CancellationToken cancellationToken = default)
        {
            var query = VisibleTags().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalizedSearch = TagNameNormalizer.NormalizeKey(search);
                query = query.Where(tag => tag.NormalizedName.Contains(normalizedSearch));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.OrderBy(tag => tag.Name).Skip((page - 1) * size).Take(size).Select(tag => new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                PostCount = tag.Posts.Count(post => post.Status == PostStatus.Published && post.isPublished && post.isActive && !post.isDeleted)
            }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public Task<TagDto?> GetDtoByIdAsync(int id, CancellationToken cancellationToken = default) => VisibleTags().AsNoTracking().Where(tag => tag.Id == id).Select(tag => new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            PostCount = tag.Posts.Count(post => post.Status == PostStatus.Published && post.isPublished && post.isActive && !post.isDeleted)
        }).FirstOrDefaultAsync(cancellationToken);

        public Task<Tag?> GetVisibleByIdAsync(int id, CancellationToken cancellationToken = default) => VisibleTags().FirstOrDefaultAsync(tag => tag.Id == id, cancellationToken);

        public Task<bool> ExistsByNormalizedNameAsync(string normalizedName, int? excludeId = null, CancellationToken cancellationToken = default)
        {
            var query = VisibleTags().AsNoTracking().Where(tag => tag.NormalizedName == normalizedName);
            if (excludeId.HasValue)
            {
                query = query.Where(tag => tag.Id != excludeId.Value);
            }

            return query.AnyAsync(cancellationToken);
        }

        private IQueryable<Tag> VisibleTags() => _context.Tags.Where(tag => tag.isActive && !tag.isDeleted);
    }
}
