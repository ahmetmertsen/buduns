using FluentValidation;

namespace buduns_server.Application.Features.Likes.Queries.GetById
{
    public class GetLikeByIdQueryValidator : AbstractValidator<GetLikeByIdQuery>
    {
        public GetLikeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
