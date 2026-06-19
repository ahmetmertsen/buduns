using blogapp_server.Application.Repositories.Common;
using blogapp_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Repositories
{
    public interface IEndpointRepository : IRepository<Endpoint>
    {
        Task<Endpoint?> GetEndpointWithMenuByCodeAsync(string code, string menu);
        Task<Endpoint?> GetRolesToEndpointWithMenu(string code, string menu);
        Task<Endpoint?> GetRolesToEndpoint(string code);
    }
}
