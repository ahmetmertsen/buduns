using FluentValidation;

namespace blogapp_server.Application.Features.Users.Commands.Update.UpdateEmail
{
    public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
    {
        public UpdateUserEmailCommandValidator()
        {
            RuleFor(x => x.ChangeEmailToken)
                .NotEmpty().WithMessage("Email değiştirme token bilgisi boş olamaz.");

            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("Yeni email boş olamaz.")
                .EmailAddress().WithMessage("Yeni email formatı geçersiz.");
        }
    }
}
