using AutoMapper;
using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Dtos.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Roles.Queries.GetAll
{
    public class GetAllRolesRequestHandler : IRequestHandler<GetAllRolesRequest, List<RoleDto>>
    {
        private readonly IRoleService _roleService;

        public GetAllRolesRequestHandler(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
        }

        public async Task<List<RoleDto>> Handle(GetAllRolesRequest request, CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllRoles();
            return roles;
        }
    }
}
