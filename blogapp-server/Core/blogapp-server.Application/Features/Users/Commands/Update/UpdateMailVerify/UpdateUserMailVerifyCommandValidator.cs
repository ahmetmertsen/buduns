using FluentValidation;

namespace blogapp_server.Application.Features.Users.Commands.Update.UpdateMailVerify
{
    public class UpdateUserMailVerifyCommandValidator : AbstractValidator<UpdateUserMailVerifyCommand>
    {
        public UpdateUserMailVerifyCommandValidator()
        {
            RuleFor(x => x.EmailConfirmToken)
                .NotEmpty().WithMessage("Email doğrulama token bilgisi boş olamaz.");
        }
    }
}
