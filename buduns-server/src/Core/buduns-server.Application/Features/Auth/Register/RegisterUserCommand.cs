using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Auth.Register
{
    public record RegisterUserCommand(string UserName, string FullName, string Email, string Password) : IRequest<RegisterUserCommandResponse>
    {
    }
}
