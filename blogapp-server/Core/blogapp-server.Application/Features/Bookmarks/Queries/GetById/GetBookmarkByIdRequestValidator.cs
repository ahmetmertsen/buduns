using FluentValidation;

namespace blogapp_server.Application.Features.Bookmarks.Queries.GetById
{
    public class GetBookmarkByIdRequestValidator : AbstractValidator<GetBookmarkByIdRequest>
    {
        public GetBookmarkByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
