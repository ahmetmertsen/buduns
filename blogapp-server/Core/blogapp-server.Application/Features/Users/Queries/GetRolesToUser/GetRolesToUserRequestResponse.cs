using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Users.Queries.GetRolesToUser
{
    public class GetRolesToUserRequestResponse
    {
        public int UserId { get; set; }
        public string[] Roles { get; set; } = null!;
    }
}
