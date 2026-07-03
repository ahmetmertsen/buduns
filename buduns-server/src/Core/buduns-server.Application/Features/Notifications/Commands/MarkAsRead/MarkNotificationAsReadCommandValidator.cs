using FluentValidation;

namespace buduns_server.Application.Features.Notifications.Commands.MarkAsRead
{
    public class MarkNotificationAsReadCommandValidator : AbstractValidator<MarkNotificationAsReadCommand>
    {
        public MarkNotificationAsReadCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
