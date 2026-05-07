using FluentValidation;

namespace blogapp_server.Application.Features.Auth.Login
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.UsernameOrEmail)
                .NotEmpty().WithMessage("Kullanıcı adı veya email boş olamaz.")
                .MaximumLength(100).WithMessage("Kullanıcı adı veya email en fazla 100 karakter olabilir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
        }
    }
}
