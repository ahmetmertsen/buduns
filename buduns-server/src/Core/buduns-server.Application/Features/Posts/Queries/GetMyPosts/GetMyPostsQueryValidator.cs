using FluentValidation;

namespace buduns_server.Application.Features.Posts.Queries.GetMyPosts
{
    public class GetMyPostsQueryValidator : AbstractValidator<GetMyPostsQuery>
    {
        public GetMyPostsQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1).WithMessage("Page 1 veya daha büyük olmalıdır.");
            RuleFor(query => query.Size).InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
        }
    }
}
