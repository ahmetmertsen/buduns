using blogapp_server.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Users.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, AssignRoleToUserCommandResponse>
    {
        private readonly IUserService _userService;

        public AssignRoleToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AssignRoleToUserCommandResponse> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.AssignRoleToUserAsync(request.UserId, request.Roles);
            AssignRoleToUserCommandResponse response = new()
            {
                Succeeded = true,
                Message = "Kullanıcıya roller atandı."
            };
            return response;
        }
    }
}
