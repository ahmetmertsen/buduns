using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Roles.Commands.Update
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleCommandResponse>
    {
        private readonly IRoleService _roleService;

        public UpdateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetRoleById(request.Id);
            if (role == null)
            {
                throw new NotFoundException("Rol bulanamadı.");
            }
            await _roleService.UpdateRole(request.Id, request.Name);

            return new UpdateRoleCommandResponse
            {
                Succeeded = true,
                Message = "Rol başarıyla güncellendi"
            };
        }
    }
}
