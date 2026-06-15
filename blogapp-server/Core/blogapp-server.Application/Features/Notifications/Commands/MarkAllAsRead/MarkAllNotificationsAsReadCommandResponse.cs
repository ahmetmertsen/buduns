namespace blogapp_server.Application.Features.Notifications.Commands.MarkAllAsRead
{
    public record MarkAllNotificationsAsReadCommandResponse(bool Succeeded, string Message, int UpdatedCount);
}
