using FluentValidation;

namespace blogapp_server.Application.Features.Posts.Queries.GetPostsByUserId
{
    public class GetPostsByUserIdQueryValidator : AbstractValidator<GetPostsByUserIdQuery>
    {
        public GetPostsByUserIdQueryValidator()
        {
            RuleFor(query => query.UserId).GreaterThan(0).WithMessage("User Id 0'dan büyük olmalıdır.");
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1).WithMessage("Page 1 veya daha büyük olmalıdır.");
            RuleFor(query => query.Size).InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
        }
    }
}
