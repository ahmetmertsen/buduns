using buduns_server.Application.Repositories;
using buduns_server.Domain.Entities;
using buduns_server.Persistence.Context;
using buduns_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Persistence.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly BudunsDbContext _context;

        public MenuRepository(BudunsDbContext context) : base(context) { _context = context; }

        public async Task<Menu?> GetMenuByNameAsync(string name) => await _context.Menus
            .FirstOrDefaultAsync(m => m.Name == name);
    }
}
