using buduns_server.Application.Abstractions.Services;
using buduns_server.Application.Dtos;
using buduns_server.Application.Repositories;
using buduns_server.Domain.Entities;
using buduns_server.Domain.Enums;

namespace buduns_server.Persistence.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification?> BuildAsync(NotificationCreateModel model, CancellationToken cancellationToken)
        {
            if (model.ActorUserId.HasValue && model.ActorUserId.Value == model.UserId)
            {
                return null;
            }

            if (model.Cooldown.HasValue)
            {
                var createdAfter = DateTime.UtcNow.Subtract(model.Cooldown.Value);
                var exists = await _notificationRepository.ExistsRecentAsync(model.Type, model.UserId, model.ActorUserId, model.PostId, model.CommentId, createdAfter, cancellationToken);
                if (exists)
                {
                    return null;
                }
            }

            var now = DateTime.UtcNow;
            return new Notification
            {
                Type = model.Type,
                Message = model.Message ?? GetDefaultMessage(model.Type),
                UserId = model.UserId,
                ActorUserId = model.ActorUserId,
                PostId = model.PostId,
                CommentId = model.CommentId,
                Comment = model.Comment,
                IsRead = false,
                CreatedAt = now,
                UpdateAt = now,
                isActive = true,
                isDeleted = false
            };
        }

        public async Task<Notification?> AddAsync(NotificationCreateModel model, CancellationToken cancellationToken)
        {
            var notification = await BuildAsync(model, cancellationToken);
            if (notification != null)
            {
                await _notificationRepository.AddAsync(notification);
            }

            return notification;
        }

        private static string GetDefaultMessage(NotificationType type) => type switch
        {
            NotificationType.NEW_FOLLOWER => "Sizi takip etmeye başladı.",
            NotificationType.POST_LIKED => "Paylaşımınız yeni bir beğeni aldı.",
            NotificationType.POST_COMMENTED => "Paylaşımınız yeni bir yorum aldı.",
            NotificationType.REPORT_RESOLVED => "Oluşturduğunuz bir şikayet moderasyon ekibi tarafından sonuçlandırıldı.",
            NotificationType.MODERATION_WARNING => "Hesabınız bir topluluk kuralı ihlali nedeniyle uyarıldı.",
            NotificationType.ACCOUNT_SUSPENDED => "Hesabınız moderasyon kararıyla askıya alındı.",
            NotificationType.ACCOUNT_BANNED => "Hesabınız moderasyon kararıyla kalıcı olarak yasaklandı.",
            NotificationType.POST_HIDDEN => "Paylaşımınız moderasyon kararıyla gizlendi.",
            NotificationType.POST_REMOVED => "Paylaşımınız moderasyon kararıyla kaldırıldı.",
            NotificationType.COMMENT_HIDDEN => "Yorumunuz moderasyon kararıyla gizlendi.",
            NotificationType.COMMENT_REMOVED => "Yorumunuz moderasyon kararıyla kaldırıldı.",
            _ => "Yeni bir bildiriminiz var."
        };
    }
}
