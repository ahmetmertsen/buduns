using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.AuthorizationEndpoint.Queries.GetRolesToEndpoint
{
    public class GetRolesToEndpointRequestValidator : AbstractValidator<GetRolesToEndpointRequest>  
    {
        GetRolesToEndpointRequestValidator() 
        {
            RuleFor(r => r.Code)
                .NotEmpty().WithMessage("Code alanı boş olamaz.");
            RuleFor(r => r.Menu)
                .NotEmpty().WithMessage("Menu alanı boş olamaz.");
        }
    }
}
