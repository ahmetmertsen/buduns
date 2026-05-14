using blogapp_server.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Users.Queries.GetRolesToUser
{
    public class GetRolesToUserRequestHandler : IRequestHandler<GetRolesToUserRequest, GetRolesToUserRequestResponse>
    {
        private readonly IUserService _userService;

        public GetRolesToUserRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetRolesToUserRequestResponse> Handle(GetRolesToUserRequest request, CancellationToken cancellationToken)
        {
            var roles = await _userService.GetRolesToUserAsync(request.UserId.ToString());
            GetRolesToUserRequestResponse response = new()
            {
                UserId = request.UserId,
                Roles = roles
            };
            return response;
        }
    }
}
