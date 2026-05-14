using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Dtos.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Roles.Queries.GetById
{
    public class GetRoleByIdRequestHandler : IRequestHandler<GetRoleByIdRequest, RoleDto>
    {
        private readonly IRoleService _roleService;

        public GetRoleByIdRequestHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<RoleDto> Handle(GetRoleByIdRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetRoleById(request.Id);
            return role;
        }
    }
}
