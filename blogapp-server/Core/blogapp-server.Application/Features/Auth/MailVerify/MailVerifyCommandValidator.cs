using FluentValidation;

namespace blogapp_server.Application.Features.Auth.MailVerify
{
    public class MailVerifyCommandValidator : AbstractValidator<MailVerifyCommand>
    {
        public MailVerifyCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Email formatı geçersiz.");
        }
    }
}
