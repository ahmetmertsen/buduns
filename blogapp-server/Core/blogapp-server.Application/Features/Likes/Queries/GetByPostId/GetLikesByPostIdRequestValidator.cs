using FluentValidation;

namespace blogapp_server.Application.Features.Likes.Queries.GetByPostId
{
    public class GetLikesByPostIdRequestValidator : AbstractValidator<GetLikesByPostIdRequest>
    {
        public GetLikesByPostIdRequestValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
        }
    }
}
