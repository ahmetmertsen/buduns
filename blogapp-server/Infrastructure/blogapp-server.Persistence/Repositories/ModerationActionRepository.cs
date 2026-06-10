using blogapp_server.Application.Repositories;
using blogapp_server.Domain.Entities;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace blogapp_server.Persistence.Repositories
{
    public class ModerationActionRepository : Repository<ModerationAction>, IModerationActionRepository
    {
        private readonly BlogAppDbContext _context;

        public ModerationActionRepository(BlogAppDbContext context) : base(context)
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
