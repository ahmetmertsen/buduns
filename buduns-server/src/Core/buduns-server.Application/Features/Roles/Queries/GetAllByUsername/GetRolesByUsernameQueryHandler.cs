using buduns_server.Application.Abstractions.Services;
using buduns_server.Application.Dtos.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Roles.Queries.GetAllByUsername
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
