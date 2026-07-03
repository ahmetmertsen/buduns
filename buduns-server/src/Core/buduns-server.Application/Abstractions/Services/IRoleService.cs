using buduns_server.Application.Dtos.Role;

namespace buduns_server.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRoles(CancellationToken cancellationToken);
        Task<List<RoleDto>> GetRolesByUsername(string userName, CancellationToken cancellationToken);
        Task<RoleDto> GetRoleById(int id, CancellationToken cancellationToken);
        Task CreateRole(string name, CancellationToken cancellationToken);
        Task DeleteRole(int id, CancellationToken cancellationToken);
        Task UpdateRole(int id, string name, CancellationToken cancellationToken);
    }
}
