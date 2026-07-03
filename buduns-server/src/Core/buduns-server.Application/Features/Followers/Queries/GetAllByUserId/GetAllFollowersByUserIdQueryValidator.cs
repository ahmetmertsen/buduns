using FluentValidation;

namespace buduns_server.Application.Features.Followers.Queries.GetAllByUserId
{
    public class GetAllFollowersByUserIdQueryValidator : AbstractValidator<GetAllFollowersByUserIdQuery>
    {
        public GetAllFollowersByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User Id 0'dan büyük olmalıdır.");
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page en az 1 olmalıdır.");
            RuleFor(x => x.Size).InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
        }
    }
}
