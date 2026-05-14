using blogapp_server.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Roles.Commands.Create
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleCommandResponse>
    {
        private readonly IRoleService _roleService;

        public CreateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleService.CreateRole(request.Name);

            CreateRoleCommandResponse response = new()
            {
                Succeeded = true,
                Message = ""
            };
            if (result)
            {
                response.Succeeded = true;
                response.Message = "Role başarıyla eklendi";
            }
            else
            {
                response.Succeeded = false;
                response.Message = "Role ekleme sırasında hata oluştu";
            }
            return response;
        }
    }
}
