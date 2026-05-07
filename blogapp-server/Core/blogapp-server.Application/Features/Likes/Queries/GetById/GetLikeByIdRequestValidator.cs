using FluentValidation;

namespace blogapp_server.Application.Features.Likes.Queries.GetById
{
    public class GetLikeByIdRequestValidator : AbstractValidator<GetLikeByIdRequest>
    {
        public GetLikeByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
