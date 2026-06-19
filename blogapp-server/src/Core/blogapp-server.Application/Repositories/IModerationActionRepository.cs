using blogapp_server.Application.Repositories.Common;
using blogapp_server.Domain.Entities;

namespace blogapp_server.Application.Repositories
{
    public interface IModerationActionRepository : IRepository<ModerationAction>
    {
        Task<List<ModerationAction>> GetByReportIdAsync(int reportId);
    }
}
