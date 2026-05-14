using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Abstractions.Services
{
    public interface IAuthorizationEndpointService
    {
        Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type);
        Task<List<string>> GetRolesToEndpoint(string code, string menu);
    }
}
