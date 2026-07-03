using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Users.Queries.GetRolesToUser
{
    public class GetRolesToUserQuery : IRequest<GetRolesToUserQueryResponse>
    {
        public int UserId { get; set; }
    }
}
