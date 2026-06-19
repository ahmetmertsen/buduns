using blogapp_server.Application.Repositories;
using blogapp_server.Domain.Entities;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Persistence.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly BlogAppDbContext _context;

        public MenuRepository(BlogAppDbContext context) : base(context) { _context = context; }

        public async Task<Menu?> GetMenuByNameAsync(string name) => await _context.Menus
            .FirstOrDefaultAsync(m => m.Name == name);
    }
}
