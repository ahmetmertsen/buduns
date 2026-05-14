using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Roles.Commands.Delete
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, DeleteRoleCommandResponse>
    {
        private readonly IRoleService _roleService;

        public DeleteRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetRoleById(request.Id);
            if (role == null)
            {
                throw new NotFoundException("Role bulunamadı.");
            }

            await _roleService.DeleteRole(request.Id);
            return new DeleteRoleCommandResponse
            {
                Succeeded = true,
                Message = "Role başarıyla silindi."
            };
        }
    }
}
