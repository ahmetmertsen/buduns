using AutoMapper;
using buduns_server.Application.Dtos.Role;
using buduns_server.Application.Features.Roles.Commands.Create;
using buduns_server.Application.Features.Roles.Commands.Update;
using buduns_server.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Mapping
{
    public class RoleProfile : Profile
    {
        public RoleProfile() 
        {
            CreateMap<CreateRoleCommand, Role>();
            CreateMap<UpdateRoleCommand, Role>();

            CreateMap<Role, RoleDto>();
        }
    }
}
