using FluentValidation;

namespace blogapp_server.Application.Features.Followers.Queries.GetById
{
    public class GetFollowerByIdQueryValidator : AbstractValidator<GetFollowerByIdQuery>
    {
        public GetFollowerByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
