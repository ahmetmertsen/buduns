using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Dtos.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Roles.Queries.GetAllByUsername
{
    public class GetRolesByUsernameQueryHandler : IRequestHandler<GetRolesByUsernameQuery, List<RoleDto>>
    {
        private readonly IRoleService _roleService;

        public GetRolesByUsernameQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<List<RoleDto>> Handle(GetRolesByUsernameQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetRolesByUsername(request.UserName, cancellationToken);
            return roles;
        }
    }
}
