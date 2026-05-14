using blogapp_server.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.AuthorizationEndpoint.Queries.GetRolesToEndpoint
{
    public class GetRolesToEndpointRequestHandler : IRequestHandler<GetRolesToEndpointRequest, GetRolesToEndpointRequestResponse>
    {
        private readonly IAuthorizationEndpointService _authorizationEndpointService;

        public GetRolesToEndpointRequestHandler(IAuthorizationEndpointService authorizationEndpointService)
        {
            _authorizationEndpointService = authorizationEndpointService;
        }

        public async Task<GetRolesToEndpointRequestResponse> Handle(GetRolesToEndpointRequest request, CancellationToken cancellationToken)
        {
            var response = await _authorizationEndpointService.GetRolesToEndpoint(request.Code, request.Menu);
            return new()
            {
                Roles = response
            };
        }
    }
}
