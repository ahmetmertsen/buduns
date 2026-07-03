using FluentValidation;

namespace buduns_server.Application.Features.Auth.RevokeSession
{
    public class RevokeSessionCommandValidator : AbstractValidator<RevokeSessionCommand>
    {
        public RevokeSessionCommandValidator()
        {
            RuleFor(command => command.SessionId)
                .NotEmpty().WithMessage("Oturum kimliği boş olamaz.");
        }
    }
}
