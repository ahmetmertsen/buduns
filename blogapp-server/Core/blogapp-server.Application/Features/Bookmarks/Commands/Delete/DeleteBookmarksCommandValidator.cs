using FluentValidation;

namespace blogapp_server.Application.Features.Bookmarks.Commands.Delete
{
    public class DeleteBookmarksCommandValidator : AbstractValidator<DeleteBookmarksCommand>
    {
        public DeleteBookmarksCommandValidator()
        {
            RuleFor(command => command.PostId)
                .GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
        }
    }
}
