using FluentValidation;

namespace blogapp_server.Application.Features.Bookmarks.Queries.GetById
{
    public class GetBookmarkByIdQueryValidator : AbstractValidator<GetBookmarkByIdQuery>
    {
        public GetBookmarkByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
