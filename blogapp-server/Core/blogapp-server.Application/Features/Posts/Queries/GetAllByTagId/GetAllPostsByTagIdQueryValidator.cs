using FluentValidation;

namespace blogapp_server.Application.Features.Posts.Queries.GetAllByTagId
{
    public class GetAllPostsByTagIdQueryValidator : AbstractValidator<GetAllPostsByTagIdQuery>
    {
        public GetAllPostsByTagIdQueryValidator()
        {
            RuleFor(x => x.TagId)
                .GreaterThan(0).WithMessage("Tag Id 0'dan büyük olmalıdır.");
        }
    }
}
