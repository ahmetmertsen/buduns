using FluentValidation;

namespace blogapp_server.Application.Features.Followers.Commands.Delete
{
    public class DeleteFollowersCommandValidator : AbstractValidator<DeleteFollowersCommand>
    {
        public DeleteFollowersCommandValidator()
        {
            RuleFor(x => x.FollowingId).GreaterThan(0).WithMessage("Following Id 0'dan büyük olmalıdır.");
        }
    }
}
