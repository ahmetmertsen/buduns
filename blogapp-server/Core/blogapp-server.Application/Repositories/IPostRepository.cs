using blogapp_server.Application.Repositories.Common;
using blogapp_server.Application.Features.Posts.Queries.GetDailyTopPosts;
using blogapp_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using blogapp_server.Application.Dtos;

namespace blogapp_server.Application.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<List<Post?>> GetAllByTagIdAsync(int TagId);
        Task<Post?> GetByIdWithTagsAsync(int id);
        Task<bool> ExistsVisibleAsync(int id, CancellationToken cancellationToken = default);
        Task<int?> GetVisibleOwnerIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TopPostDto>> GetDailyTopPostsAsync(DateTime startDateUtc, DateTime endDateUtc, int limit, CancellationToken cancellationToken = default);
    }
}
