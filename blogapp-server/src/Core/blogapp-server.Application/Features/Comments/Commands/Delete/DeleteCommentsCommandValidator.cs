using FluentValidation;

namespace blogapp_server.Application.Features.Comments.Commands.Delete
{
    public class DeleteCommentsCommandValidator : AbstractValidator<DeleteCommentsCommand>
    {
        public DeleteCommentsCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
