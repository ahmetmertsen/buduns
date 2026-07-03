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
    public class EndpointRepository : Repository<Endpoint>, IEndpointRepository
    {
        private readonly BudunsDbContext _context;

        public EndpointRepository(BudunsDbContext context) : base(context) { _context = context; }

        public async Task<Endpoint?> GetEndpointWithMenuByCodeAsync(string code, string menu) => await _context.Endpoints
            .Include(e => e.Menu)
            .Include(e => e.Roles)
            .FirstOrDefaultAsync(c => c.Code == code && c.Menu.Name == menu);

        public async Task<Endpoint?> GetRolesToEndpointWithMenu(string code, string menu) => await _context.Endpoints
            .Include(e => e.Roles)
            .Include(e => e.Menu)
            .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);

        public async Task<Endpoint?> GetRolesToEndpoint(string code) => await _context.Endpoints
            .Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Code == code);
    }
}
