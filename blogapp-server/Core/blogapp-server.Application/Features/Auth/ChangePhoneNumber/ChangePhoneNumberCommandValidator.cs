using FluentValidation;

namespace blogapp_server.Application.Features.Auth.ChangePhoneNumber
{
    public class ChangePhoneNumberCommandValidator : AbstractValidator<ChangePhoneNumberCommand>
    {
        public ChangePhoneNumberCommandValidator()
        {
            RuleFor(x => x.NewPhoneNumber)
                .NotEmpty().WithMessage("Yeni telefon numarası boş olamaz.")
                .MaximumLength(20).WithMessage("Yeni telefon numarası en fazla 20 karakter olabilir.");
        }
    }
}
