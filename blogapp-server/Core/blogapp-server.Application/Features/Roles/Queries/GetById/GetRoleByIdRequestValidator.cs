using FluentValidation;

namespace blogapp_server.Application.Features.Roles.Queries.GetById
{
    public class GetRoleByIdRequestValidator : AbstractValidator<GetRoleByIdRequest>
    {
        public GetRoleByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
