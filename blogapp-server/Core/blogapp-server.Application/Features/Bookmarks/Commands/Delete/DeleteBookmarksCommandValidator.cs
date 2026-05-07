using FluentValidation;

namespace blogapp_server.Application.Features.Bookmarks.Commands.Delete
{
    public class DeleteBookmarksCommandValidator : AbstractValidator<DeleteBookmarksCommand>
    {
        public DeleteBookmarksCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
