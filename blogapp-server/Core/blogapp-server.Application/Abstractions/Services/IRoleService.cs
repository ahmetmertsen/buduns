using blogapp_server.Application.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRoles();
        Task<List<RoleDto>> GetRolesByUsername(string userName);
        Task<RoleDto> GetRoleById(int id);
        Task<bool> CreateRole(string name);
        Task<bool> DeleteRole(int id);
        Task<bool> UpdateRole(int id, string name);
    }
}
