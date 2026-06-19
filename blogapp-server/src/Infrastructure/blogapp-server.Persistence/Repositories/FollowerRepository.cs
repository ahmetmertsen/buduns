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
    public class FollowerRepository : Repository<Follower>, IFollowerRepository
    {
        private const string UserFollowUniqueIndex = "IX_Followers_FollowerId_FollowingId";
        private readonly BlogAppDbContext _context;

        public FollowerRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(Follower Follower, bool Created)> CreateIfNotExistsAsync(Follower follower, Notification? notification, CancellationToken cancellationToken)
        {
            var existingFollow = await _context.Followers.FirstOrDefaultAsync(item => item.FollowerId == follower.FollowerId && item.FollowingId == follower.FollowingId, cancellationToken);
            if (existingFollow != null)
            {
                if (existingFollow.isActive && !existingFollow.isDeleted)
                {
                    return (existingFollow, false);
                }

                existingFollow.isActive = true;
                existingFollow.isDeleted = false;
                existingFollow.CreatedAt = DateTime.UtcNow;
                if (notification != null)
                {
                    await _context.Notifications.AddAsync(notification, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return (existingFollow, true);
            }

            await _context.Followers.AddAsync(follower, cancellationToken);
            if (notification != null)
            {
                await _context.Notifications.AddAsync(notification, cancellationToken);
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return (follower, true);
            }
            catch (DbUpdateException exception) when (IsDuplicateFollow(exception))
            {
                _context.Entry(follower).State = EntityState.Detached;
                if (notification != null)
                {
                    _context.Entry(notification).State = EntityState.Detached;
                }

                existingFollow = await _context.Followers.AsNoTracking().FirstOrDefaultAsync(item => item.FollowerId == follower.FollowerId && item.FollowingId == follower.FollowingId, cancellationToken);
                if (existingFollow != null)
                {
                    return (existingFollow, false);
                }

                throw;
            }
        }

        public async Task<bool> DeleteByUsersAsync(int followerId, int followingId, CancellationToken cancellationToken)
        {
            var follow = await _context.Followers.FirstOrDefaultAsync(item => item.FollowerId == followerId && item.FollowingId == followingId, cancellationToken);
            if (follow == null)
            {
                return false;
            }

            _context.Followers.Remove(follow);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<Follower?> GetByUsersAsync(int followerId, int followingId, CancellationToken cancellationToken) => _context.Followers.AsNoTracking().FirstOrDefaultAsync(item => item.FollowerId == followerId && item.FollowingId == followingId && item.isActive && !item.isDeleted, cancellationToken);

        public async Task<(List<FollowerDto> Items, int TotalCount)> GetPagedFollowersByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken)
        {
            var query = _context.Followers.Where(follow => follow.FollowingId == userId && follow.isActive && !follow.isDeleted && follow.FollowerUser.Status != UserStatus.Banned);
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(follow => follow.CreatedAt).ThenByDescending(follow => follow.Id).Skip((page - 1) * size).Take(size).AsNoTracking().Select(follow => new FollowerDto
            {
                Id = follow.Id,
                UserId = follow.FollowerId,
                UserName = follow.FollowerUser.UserName!,
                FullName = follow.FollowerUser.FullName,
                Bio = follow.FollowerUser.Bio,
                ImageUrl = follow.FollowerUser.ImageUrl,
                FollowedAt = follow.CreatedAt
            }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<FollowerDto> Items, int TotalCount)> GetPagedFollowingsByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken)
        {
            var query = _context.Followers.Where(follow => follow.FollowerId == userId && follow.isActive && !follow.isDeleted && follow.FollowingUser.Status != UserStatus.Banned);
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(follow => follow.CreatedAt).ThenByDescending(follow => follow.Id).Skip((page - 1) * size).Take(size).AsNoTracking().Select(follow => new FollowerDto
            {
                Id = follow.Id,
                UserId = follow.FollowingId,
                UserName = follow.FollowingUser.UserName!,
                FullName = follow.FollowingUser.FullName,
                Bio = follow.FollowingUser.Bio,
                ImageUrl = follow.FollowingUser.ImageUrl,
                FollowedAt = follow.CreatedAt
            }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        private static bool IsDuplicateFollow(DbUpdateException exception) => exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: UserFollowUniqueIndex
        };
    }
}
