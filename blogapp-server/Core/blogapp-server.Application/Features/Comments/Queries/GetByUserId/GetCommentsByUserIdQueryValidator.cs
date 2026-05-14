using FluentValidation;

namespace blogapp_server.Application.Features.Comments.Queries.GetByUserId
{
    public class GetCommentsByUserIdQueryValidator : AbstractValidator<GetCommentsByUserIdQuery>
    {
        public GetCommentsByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User Id 0'dan büyük olmalıdır.");
        }
    }
}
