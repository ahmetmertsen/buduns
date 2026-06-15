using blogapp_server.Application.Repositories;
using blogapp_server.Domain.Entities;
using blogapp_server.Application.Dtos;
using blogapp_server.Domain.Enums;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Persistence.Repositories
{
    public class NotificationRepository : Repository<Notification> , INotificationRepository
    {
        private readonly BlogAppDbContext _context;
        private readonly IMapper _mapper;

        public NotificationRepository(BlogAppDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(List<NotificationDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, bool onlyUnread, CancellationToken cancellationToken)
        {
            var query = VisibleByUser(userId);
            if (onlyUnread)
            {
                query = query.Where(notification => !notification.IsRead);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(notification => notification.CreatedAt).ThenByDescending(notification => notification.Id).Skip((page - 1) * size).Take(size).ProjectTo<NotificationDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellationToken) => VisibleByUser(userId).CountAsync(notification => !notification.IsRead, cancellationToken);

        public async Task<bool> MarkAsReadAsync(int notificationId, int userId, CancellationToken cancellationToken)
        {
            var notification = await VisibleByUser(userId).FirstOrDefaultAsync(item => item.Id == notificationId, cancellationToken);
            if (notification == null)
            {
                return false;
            }

            if (!notification.IsRead)
            {
                var now = DateTime.UtcNow;
                notification.IsRead = true;
                notification.ReadAt = now;
                notification.UpdateAt = now;
            }

            return true;
        }

        public async Task<int> MarkAllAsReadAsync(int userId, CancellationToken cancellationToken)
        {
            var notifications = await VisibleByUser(userId).Where(notification => !notification.IsRead).ToListAsync(cancellationToken);
            var now = DateTime.UtcNow;
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = now;
                notification.UpdateAt = now;
            }

            return notifications.Count;
        }

        public async Task<bool> SoftDeleteByIdAndUserAsync(int notificationId, int userId, CancellationToken cancellationToken)
        {
            var notification = await VisibleByUser(userId).FirstOrDefaultAsync(item => item.Id == notificationId, cancellationToken);
            if (notification == null)
            {
                return false;
            }

            notification.isActive = false;
            notification.isDeleted = true;
            notification.UpdateAt = DateTime.UtcNow;
            return true;
        }

        public Task<bool> ExistsRecentAsync(NotificationType type, int userId, int? actorUserId, int? postId, int? commentId, DateTime createdAfter, CancellationToken cancellationToken) => _context.Notifications.AsNoTracking().AnyAsync(notification => notification.Type == type && notification.UserId == userId && notification.ActorUserId == actorUserId && notification.PostId == postId && notification.CommentId == commentId && notification.CreatedAt >= createdAfter && notification.isActive && !notification.isDeleted, cancellationToken);

        private IQueryable<Notification> VisibleByUser(int userId) => _context.Notifications.Where(notification => notification.UserId == userId && notification.isActive && !notification.isDeleted);
    }
}
