using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Users.Queries.GetRolesToUser
{
    public class GetRolesToUserRequestValidator : AbstractValidator<GetRolesToUserRequest>
    {
        public GetRolesToUserRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Kullanıcı bilgisi boş olamaz");
        }
    }
}
