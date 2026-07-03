using FluentValidation;

namespace buduns_server.Application.Features.Users.Commands.Update.UpdatePassword
{
    public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
    {
        public UpdateUserPasswordCommandValidator()
        {
            RuleFor(x => x.EmailOrUsername)
                .NotEmpty().WithMessage("Email veya kullanıcı adı boş olamaz.")
                .MaximumLength(100).WithMessage("Email veya kullanıcı adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.VerificationCode)
                .NotEmpty().WithMessage("Şifre sıfırlama kodu boş olamaz.")
                .Length(6).WithMessage("Şifre sıfırlama kodu 6 haneli olmalıdır.")
                .Matches("^[0-9]{6}$").WithMessage("Şifre sıfırlama kodu sadece rakamlardan oluşmalıdır.");

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
