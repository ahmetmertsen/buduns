using FluentValidation;

namespace blogapp_server.Application.Features.Auth.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandValidator : AbstractValidator<RefreshTokenLoginCommand>
    {
        public RefreshTokenLoginCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token boş olamaz.");
        }
    }
}
