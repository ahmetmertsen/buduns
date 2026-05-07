using FluentValidation;

namespace blogapp_server.Application.Features.Users.Commands.Update.UpdatePassword
{
    public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
    {
        public UpdateUserPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User Id 0'dan büyük olmalıdır.");

            RuleFor(x => x.ResetToken)
                .NotEmpty().WithMessage("Şifre sıfırlama token bilgisi boş olamaz.");

            RuleFor(x => x.newPassword)
                .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Yeni şifre en az 6 karakter olmalıdır.");

            RuleFor(x => x.newPasswordConfirmed)
                .NotEmpty().WithMessage("Yeni şifre tekrarı boş olamaz.")
                .MinimumLength(6).WithMessage("Yeni şifre tekrarı en az 6 karakter olmalıdır.")
                .Equal(x => x.newPassword).WithMessage("Şifreler uyuşmuyor.");
        }
    }
}
