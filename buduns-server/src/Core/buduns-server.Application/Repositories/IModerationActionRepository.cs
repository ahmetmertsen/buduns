using buduns_server.Application.Repositories.Common;
using buduns_server.Domain.Entities;

namespace buduns_server.Application.Repositories
{
    public interface IModerationActionRepository : IRepository<ModerationAction>
    {
        Task<List<ModerationAction>> GetByReportIdAsync(int reportId);
    }
}
