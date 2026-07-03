using FluentValidation;

namespace buduns_server.Application.Features.Bookmarks.Queries.GetStatus
{
    public class GetBookmarkStatusQueryValidator : AbstractValidator<GetBookmarkStatusQuery>
    {
        public GetBookmarkStatusQueryValidator()
        {
            RuleFor(query => query.PostId)
                .GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");
        }
    }
}
