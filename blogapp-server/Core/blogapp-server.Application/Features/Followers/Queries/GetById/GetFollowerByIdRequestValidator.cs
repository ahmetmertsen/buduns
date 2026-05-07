using FluentValidation;

namespace blogapp_server.Application.Features.Followers.Queries.GetById
{
    public class GetFollowerByIdRequestValidator : AbstractValidator<GetFollowerByIdRequest>
    {
        public GetFollowerByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
