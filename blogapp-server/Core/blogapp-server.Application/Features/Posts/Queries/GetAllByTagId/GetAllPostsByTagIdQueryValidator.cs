using FluentValidation;

namespace blogapp_server.Application.Features.Posts.Queries.GetAllByTagId
{
    public class GetAllPostsByTagIdQueryValidator : AbstractValidator<GetAllPostsByTagIdQuery>
    {
        public GetAllPostsByTagIdQueryValidator()
        {
            RuleFor(x => x.TagId).GreaterThan(0).WithMessage("Tag Id 0'dan büyük olmalıdır.");
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page 1 veya daha büyük olmalıdır.");
            RuleFor(x => x.Size).InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
        }
    }
}
