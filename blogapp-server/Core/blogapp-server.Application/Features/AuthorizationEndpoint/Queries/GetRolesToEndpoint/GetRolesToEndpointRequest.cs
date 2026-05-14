using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.AuthorizationEndpoint.Queries.GetRolesToEndpoint
{
    public class GetRolesToEndpointRequest : IRequest<GetRolesToEndpointRequestResponse>
    {
        public string Code { get; set; }
        public string Menu { get; set; }
    }
}
