using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.AuthorizationEndpoint.Commands.AssignRoleEndpoint
{
    public class AssignRoleEndpointCommandValidator : AbstractValidator<AssignRoleEndpointCommand>
    {
        public AssignRoleEndpointCommandValidator() 
        {
            RuleFor(r => r.Roles)
                .NotEmpty().WithMessage("Roller boþ olamaz.");
            RuleFor(r => r.Code)
                .NotEmpty().WithMessage("Code alaný boþ olamaz.");
            RuleFor(r => r.Menu)
                .NotEmpty().WithMessage("Menu alaný boþ olamaz.");
        }
    }
}
