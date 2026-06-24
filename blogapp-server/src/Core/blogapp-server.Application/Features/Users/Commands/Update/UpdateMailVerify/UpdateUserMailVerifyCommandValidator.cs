using FluentValidation;

namespace blogapp_server.Application.Features.Users.Commands.Update.UpdateMailVerify
{
    public class UpdateUserMailVerifyCommandValidator : AbstractValidator<UpdateUserMailVerifyCommand>
    {
        public UpdateUserMailVerifyCommandValidator()
        {
            RuleFor(x => x.VerificationCode)
                .NotEmpty().WithMessage("Email doğrulama kodu boş olamaz.")
                .Length(6).WithMessage("Email doğrulama kodu 6 haneli olmalıdır.")
                .Matches("^[0-9]{6}$").WithMessage("Email doğrulama kodu sadece rakamlardan oluşmalıdır.");
        }
    }
}
