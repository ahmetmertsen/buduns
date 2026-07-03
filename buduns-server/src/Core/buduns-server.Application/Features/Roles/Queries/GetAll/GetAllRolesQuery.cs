using buduns_server.Application.Dtos.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Roles.Queries.GetAll
{
    public class GetAllRolesQuery : IRequest<List<RoleDto>>
    {
    }
}
