using FluentValidation;

namespace blogapp_server.Application.Features.Likes.Commands.Delete
{
    public class DeleteLikesCommandValidator : AbstractValidator<DeleteLikesCommand>
    {
        public DeleteLikesCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
