using FluentValidation;

namespace buduns_server.Application.Features.Roles.Queries.GetAllByUsername
{
    public class GetRolesByUsernameQueryValidator : AbstractValidator<GetRolesByUsernameQuery>
    {
        public GetRolesByUsernameQueryValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir.");
        }
    }
}
