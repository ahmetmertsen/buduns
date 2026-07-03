using FluentValidation;

namespace buduns_server.Application.Features.Likes.Commands.Delete
{
    public class DeleteLikesCommandValidator : AbstractValidator<DeleteLikesCommand>
    {
        public DeleteLikesCommandValidator()
        {
            RuleFor(x => x.PostId).GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
        }
    }
}
