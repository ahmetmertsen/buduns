using FluentValidation;

namespace blogapp_server.Application.Features.Likes.Queries.GetByPostId
{
    public class GetLikesByPostIdQueryValidator : AbstractValidator<GetLikesByPostIdQuery>
    {
        public GetLikesByPostIdQueryValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
        }
    }
}
