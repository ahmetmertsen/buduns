using blogapp_server.Application.Repositories.Common;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Repositories
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<List<Report>> GetAllWithDetailsAsync();

        Task<Report?> GetByIdWithDetailsAsync(int id);

        Task<List<Report>> GetAllByStatusAsync(ReportStatus status);

        Task<List<Report>> GetAllByTargetTypeAsync(ReportTargetType targetType);

        Task<List<Report>> GetAllByReporterUserIdAsync(int reporterUserId);

        Task<List<Report>> GetReportsForTargetAsync(ReportTargetType targetType, int targetId, CancellationToken cancellationToken = default);

        Task<bool> HasPendingPostReportAsync(int reporterUserId, int postId, CancellationToken cancellationToken = default);

        Task<bool> HasPendingUserReportAsync(int reporterUserId, int targetUserId, CancellationToken cancellationToken = default);

        Task<bool> HasPendingCommentReportAsync(int reporterUserId, int commentId, CancellationToken cancellationToken = default);

        Task<(List<Report> Reports, int TotalCount)> GetFilteredReportGroupsAsync(ReportStatus? status, ReportTargetType? targetType, ReportReason? reason, DateTime? fromDate, DateTime? toDate, int page, int size, CancellationToken cancellationToken = default);

        Task<List<Report>> GetOpenReportsForTargetAsync(ReportTargetType targetType, int targetId, CancellationToken cancellationToken = default);

        Task<int> CountRecentReportsByUserAsync(int reporterUserId, DateTime since, CancellationToken cancellationToken = default);
    }
}
