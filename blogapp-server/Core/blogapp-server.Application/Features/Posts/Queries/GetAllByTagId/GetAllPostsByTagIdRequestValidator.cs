using FluentValidation;

namespace blogapp_server.Application.Features.Posts.Queries.GetAllByTagId
{
    public class GetAllPostsByTagIdRequestValidator : AbstractValidator<GetAllPostsByTagIdRequest>
    {
        public GetAllPostsByTagIdRequestValidator()
        {
            RuleFor(x => x.TagId)
                .GreaterThan(0).WithMessage("Tag Id 0'dan büyük olmalıdır.");
        }
    }
}
