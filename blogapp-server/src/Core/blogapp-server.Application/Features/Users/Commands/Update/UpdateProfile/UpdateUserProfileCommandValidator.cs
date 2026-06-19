using FluentValidation;

namespace blogapp_server.Application.Features.Users.Commands.Update.UpdateProfile
{
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad soyad boş olamaz.")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.");

            RuleFor(x => x.Bio)
                .MaximumLength(1000).WithMessage("Bio en fazla 1000 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).WithMessage("Image Url en fazla 500 karakter olabilir.");
        }
    }
}
