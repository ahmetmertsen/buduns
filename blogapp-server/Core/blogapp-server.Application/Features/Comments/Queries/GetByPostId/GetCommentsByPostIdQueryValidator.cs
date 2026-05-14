using FluentValidation;

namespace blogapp_server.Application.Features.Comments.Queries.GetByPostId
{
    public class GetCommentsByPostIdQueryValidator : AbstractValidator<GetCommentsByPostIdQuery>
    {
        public GetCommentsByPostIdQueryValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
        }
    }
}
