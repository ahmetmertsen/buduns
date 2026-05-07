using FluentValidation;

namespace blogapp_server.Application.Features.Users.Queries.GetById
{
    public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdRequest>
    {
        public GetUserByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
