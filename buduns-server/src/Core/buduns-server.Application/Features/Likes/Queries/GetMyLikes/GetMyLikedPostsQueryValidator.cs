using FluentValidation;

namespace buduns_server.Application.Features.Likes.Queries.GetMyLikes
{
    public class GetMyLikedPostsQueryValidator : AbstractValidator<GetMyLikedPostsQuery>
    {
        public GetMyLikedPostsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page en az 1 olmalıdır.");
            RuleFor(x => x.Size).InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
        }
    }
}
