using FluentValidation;

namespace blogapp_server.Application.Features.Followers.Queries.GetAllByUserId
{
    public class GetAllFollowersByUserIdRequestValidator : AbstractValidator<GetAllFollowersByUserIdRequest>
    {
        public GetAllFollowersByUserIdRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User Id 0'dan büyük olmalıdır.");
        }
    }
}
