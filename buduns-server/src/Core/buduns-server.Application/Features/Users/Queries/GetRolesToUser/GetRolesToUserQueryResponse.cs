using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Users.Queries.GetRolesToUser
{
    public class GetRolesToUserQueryResponse
    {
        public int UserId { get; set; }
        public string[] Roles { get; set; } = null!;
    }
}
