using FluentValidation;

namespace blogapp_server.Application.Features.Likes.Commands.Create
{
    public class CreateLikesCommandValidator : AbstractValidator<CreateLikesCommand>
    {
        public CreateLikesCommandValidator()
        {
            RuleFor(x => x.PostId).GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
        }
    }
}
