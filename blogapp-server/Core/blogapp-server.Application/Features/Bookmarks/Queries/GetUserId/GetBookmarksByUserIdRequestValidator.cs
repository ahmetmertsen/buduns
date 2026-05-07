using FluentValidation;

namespace blogapp_server.Application.Features.Bookmarks.Queries.GetUserId
{
    public class GetBookmarksByUserIdRequestValidator : AbstractValidator<GetBookmarksByUserIdRequest>
    {
        public GetBookmarksByUserIdRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User Id 0'dan büyük olmalıdır.");
        }
    }
}
