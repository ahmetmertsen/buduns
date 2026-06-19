namespace blogapp_server.Application.Features.Notifications.Commands.MarkAsRead
{
    public record MarkNotificationAsReadCommandResponse(bool Succeeded, string Message);
}
