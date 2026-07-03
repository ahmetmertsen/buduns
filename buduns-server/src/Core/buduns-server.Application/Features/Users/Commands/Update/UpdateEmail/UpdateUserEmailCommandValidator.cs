using FluentValidation;

namespace buduns_server.Application.Features.Users.Commands.Update.UpdateEmail
{
    public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
    {
        public UpdateUserEmailCommandValidator()
        {
            RuleFor(x => x.OldEmailVerificationCode)
                .NotEmpty().WithMessage("Mevcut email doğrulama kodu boş olamaz.")
                .Length(6).WithMessage("Mevcut email doğrulama kodu 6 haneli olmalıdır.")
                .Matches("^[0-9]{6}$").WithMessage("Mevcut email doğrulama kodu sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.NewEmailVerificationCode)
                .NotEmpty().WithMessage("Yeni email doğrulama kodu boş olamaz.")
                .Length(6).WithMessage("Yeni email doğrulama kodu 6 haneli olmalıdır.")
                .Matches("^[0-9]{6}$").WithMessage("Yeni email doğrulama kodu sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("Yeni email boş olamaz.")
                .EmailAddress().WithMessage("Yeni email formatı geçersiz.");
        }
    }
}
