using blogapp_server.Application.Dtos.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Roles.Queries.GetAll
{
    public class GetAllRolesRequest : IRequest<List<RoleDto>>
    {
    }
}
