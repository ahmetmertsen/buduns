using buduns_server.Application.Repositories.Common;
using buduns_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Task<Menu?> GetMenuByNameAsync(string name);
    }
}
