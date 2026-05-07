using FluentValidation;

namespace blogapp_server.Application.Features.Followers.Commands.Create
{
    public class CreateFollowersCommandValidator : AbstractValidator<CreateFollowersCommand>
    {
        public CreateFollowersCommandValidator()
        {
            RuleFor(x => x.FollowingId)
                .GreaterThan(0).WithMessage("Following Id 0'dan büyük olmalıdır.");
        }
    }
}
