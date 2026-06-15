using FluentValidation;

namespace blogapp_server.Application.Features.Likes.Queries.GetByPostId
{
    public class GetLikesByPostIdQueryValidator : AbstractValidator<GetLikesByPostIdQuery>
    {
        public GetLikesByPostIdQueryValidator()
        {
            RuleFor(x => x.PostId).GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page en az 1 olmalıdır.");
            RuleFor(x => x.Size).InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
        }
    }
}
