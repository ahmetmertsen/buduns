using FluentValidation;

namespace buduns_server.Application.Features.Auth.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.EmailOrUsername)
                .NotEmpty().WithMessage("Email veya kullanıcı adı boş olamaz.")
                .MaximumLength(100).WithMessage("Email veya kullanıcı adı en fazla 100 karakter olabilir.");
        }
    }
}
