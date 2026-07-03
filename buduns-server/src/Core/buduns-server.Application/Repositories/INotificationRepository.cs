using buduns_server.Application.Repositories.Common;
using buduns_server.Domain.Entities;
using buduns_server.Application.Dtos;
using buduns_server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<(List<NotificationDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, bool onlyUnread, CancellationToken cancellationToken);
        Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellationToken);
        Task<bool> MarkAsReadAsync(int notificationId, int userId, CancellationToken cancellationToken);
        Task<int> MarkAllAsReadAsync(int userId, CancellationToken cancellationToken);
        Task<bool> SoftDeleteByIdAndUserAsync(int notificationId, int userId, CancellationToken cancellationToken);
        Task<bool> ExistsRecentAsync(NotificationType type, int userId, int? actorUserId, int? postId, int? commentId, DateTime createdAfter, CancellationToken cancellationToken);
    }
}
