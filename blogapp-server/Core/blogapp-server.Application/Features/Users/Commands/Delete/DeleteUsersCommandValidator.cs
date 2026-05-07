using FluentValidation;

namespace blogapp_server.Application.Features.Users.Commands.Delete
{
    public class DeleteUsersCommandValidator : AbstractValidator<DeleteUsersCommand>
    {
        public DeleteUsersCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir.");
        }
    }
}
