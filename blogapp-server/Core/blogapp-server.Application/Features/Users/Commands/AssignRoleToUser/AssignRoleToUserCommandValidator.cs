using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Users.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
    {
        public AssignRoleToUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Kullanıcı bilgisi boş olamaz");
        }
    }
}
