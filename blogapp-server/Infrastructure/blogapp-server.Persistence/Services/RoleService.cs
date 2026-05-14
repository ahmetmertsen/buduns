using AutoMapper;
using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Dtos.Role;
using blogapp_server.Application.Exceptions;
using blogapp_server.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Persistence.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var response = _mapper.Map<List<RoleDto>>(roles);
            return response;
        }

        public async Task<List<RoleDto>> GetRolesByUsername(string userName)
        {
            var roles = await _roleManager.Roles.Where(r => r.Name == userName).ToListAsync();
            var response = _mapper.Map<List<RoleDto>>(roles);
            return response;
        }

        public async Task<RoleDto> GetRoleById(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new NotFoundException("Role bulunamadı.");
            }

            RoleDto roleDto = new()
            {
                Id = role.Id,
                Name = role.Name
            };
            return roleDto;
        }

        public async Task<bool> CreateRole(string name)
        {
            var result = await _roleManager.CreateAsync(new Role { Name = name });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new NotFoundException("Role bulunamadı.");
            }

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> UpdateRole(int id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new NotFoundException("Role bulunamadı.");
            }
            role.Name = name;

            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
