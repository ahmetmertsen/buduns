using FluentValidation;

namespace buduns_server.Application.Features.Posts.Commands.Delete
{
    public class DeletePostsCommandValidator : AbstractValidator<DeletePostsCommand>
    {
        public DeletePostsCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
