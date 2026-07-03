using buduns_server.Application.Dtos;
using buduns_server.Domain.Entities;

namespace buduns_server.Application.Abstractions.Services
{
    public interface INotificationService
    {
        Task<Notification?> BuildAsync(NotificationCreateModel model, CancellationToken cancellationToken);
        Task<Notification?> AddAsync(NotificationCreateModel model, CancellationToken cancellationToken);
    }
}
