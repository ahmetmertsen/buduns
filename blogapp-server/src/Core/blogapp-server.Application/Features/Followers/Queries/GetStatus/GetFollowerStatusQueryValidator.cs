using FluentValidation;

namespace blogapp_server.Application.Features.Followers.Queries.GetStatus
{
    public class GetFollowerStatusQueryValidator : AbstractValidator<GetFollowerStatusQuery>
    {
        public GetFollowerStatusQueryValidator()
        {
            RuleFor(x => x.FollowingId).GreaterThan(0).WithMessage("Following Id 0'dan büyük olmalıdır.");
        }
    }
}
