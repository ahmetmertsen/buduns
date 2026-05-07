using FluentValidation;

namespace blogapp_server.Application.Features.Posts.Queries.GetById
{
    public class GetPostByIdRequestValidator : AbstractValidator<GetPostByIdRequest>
    {
        public GetPostByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
