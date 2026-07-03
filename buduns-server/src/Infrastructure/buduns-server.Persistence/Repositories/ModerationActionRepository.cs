using buduns_server.Application.Repositories;
using buduns_server.Domain.Entities;
using buduns_server.Persistence.Context;
using buduns_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace buduns_server.Persistence.Repositories
{
    public class ModerationActionRepository : Repository<ModerationAction>, IModerationActionRepository
    {
        private readonly BudunsDbContext _context;

        public ModerationActionRepository(BudunsDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ModerationAction>> GetByReportIdAsync(int reportId) => _context.ModerationActions
            .AsNoTracking()
            .Include(action => action.ModeratorUser)
            .Where(action => action.ReportId == reportId)
            .OrderByDescending(action => action.CreatedAt)
            .ToListAsync();
    }
}
