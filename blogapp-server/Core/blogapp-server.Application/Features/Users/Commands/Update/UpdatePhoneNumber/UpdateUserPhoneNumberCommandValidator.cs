using FluentValidation;

namespace blogapp_server.Application.Features.Users.Commands.Update.UpdatePhoneNumber
{
    public class UpdateUserPhoneNumberCommandValidator : AbstractValidator<UpdateUserPhoneNumberCommand>
    {
        public UpdateUserPhoneNumberCommandValidator()
        {
            RuleFor(x => x.NewPhoneNumber)
                .NotEmpty().WithMessage("Yeni telefon numarası boş olamaz.")
                .MaximumLength(20).WithMessage("Yeni telefon numarası en fazla 20 karakter olabilir.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Telefon numarası değiştirme token bilgisi boş olamaz.");
        }
    }
}
