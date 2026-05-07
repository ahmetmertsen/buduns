using FluentValidation;

namespace blogapp_server.Application.Features.Likes.Queries.GetByUserId
{
    public class GetLikesByUserIdRequestValidator : AbstractValidator<GetLikesByUserIdRequest>
    {
        public GetLikesByUserIdRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User Id 0'dan büyük olmalıdır.");
        }
    }
}
