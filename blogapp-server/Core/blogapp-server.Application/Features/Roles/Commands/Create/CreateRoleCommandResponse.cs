using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Roles.Commands.Create
{
    public class CreateRoleCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
    }
}
