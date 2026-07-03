using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Auth.Login
{
    public record LoginUserCommand(string UsernameOrEmail, string Password) : IRequest<LoginUserCommandResponse>
    {
    }
}
