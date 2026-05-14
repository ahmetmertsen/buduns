using FluentValidation;

namespace blogapp_server.Application.Features.Roles.Commands.Update
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Rol adı boş olamaz.")
                .MaximumLength(100).WithMessage("Rol adı en fazla 100 karakter olabilir.");
        }
    }
}
