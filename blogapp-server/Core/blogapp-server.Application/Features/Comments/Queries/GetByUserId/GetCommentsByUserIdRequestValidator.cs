using FluentValidation;

namespace blogapp_server.Application.Features.Comments.Queries.GetByUserId
{
    public class GetCommentsByUserIdRequestValidator : AbstractValidator<GetCommentsByUserIdRequest>
    {
        public GetCommentsByUserIdRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User Id 0'dan büyük olmalıdır.");
        }
    }
}
