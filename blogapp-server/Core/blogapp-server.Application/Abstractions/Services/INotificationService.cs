using blogapp_server.Application.Dtos;
using blogapp_server.Domain.Entities;

namespace blogapp_server.Application.Abstractions.Services
{
    public interface INotificationService
    {
        Task<Notification?> BuildAsync(NotificationCreateModel model, CancellationToken cancellationToken);
        Task<Notification?> AddAsync(NotificationCreateModel model, CancellationToken cancellationToken);
    }
}
