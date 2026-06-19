using FluentValidation;

namespace blogapp_server.Application.Features.Likes.Queries.GetStatus
{
    public class GetLikeStatusQueryValidator : AbstractValidator<GetLikeStatusQuery>
    {
        public GetLikeStatusQueryValidator()
        {
            RuleFor(x => x.PostId).GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
        }
    }
}
