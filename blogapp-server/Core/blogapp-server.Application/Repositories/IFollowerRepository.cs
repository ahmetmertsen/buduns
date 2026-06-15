using blogapp_server.Application.Dtos;
using blogapp_server.Application.Repositories.Common;
using blogapp_server.Domain.Entities;

namespace blogapp_server.Application.Repositories
{
    public interface IFollowerRepository : IRepository<Follower>
    {
        Task<(Follower Follower, bool Created)> CreateIfNotExistsAsync(Follower follower, Notification? notification, CancellationToken cancellationToken);
        Task<bool> DeleteByUsersAsync(int followerId, int followingId, CancellationToken cancellationToken);
        Task<Follower?> GetByUsersAsync(int followerId, int followingId, CancellationToken cancellationToken);
        Task<(List<FollowerDto> Items, int TotalCount)> GetPagedFollowersByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken);
        Task<(List<FollowerDto> Items, int TotalCount)> GetPagedFollowingsByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken);
    }
}
