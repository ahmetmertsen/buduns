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
    public class UtilityRepository : Repository<Utility>, IUtilityRepository
    {
        private readonly BudunsDbContext _context;

        public UtilityRepository(BudunsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Utility?> GetByNameAsync(string name)
        {
            return await _context.Utilities.FirstOrDefaultAsync(u => u.Name == name);
        }
    }
}
