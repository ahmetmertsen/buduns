using blogapp_server.Application.Repositories;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace blogapp_server.Persistence.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly BlogAppDbContext _context;

        public ReportRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public override Task<List<Report>> GetAllAsync() => _context.Reports.ToListAsync();

        public override Task<Report?> GetByIdAsync(int id) => _context.Reports.FirstOrDefaultAsync(report => report.Id == id);

        public Task<List<Report>> GetAllWithDetailsAsync() => Details().OrderByDescending(report => report.CreatedAt).ToListAsync();

        public Task<Report?> GetByIdWithDetailsAsync(int id) => Details().Include(report => report.ModerationActions).ThenInclude(action => action.ModeratorUser).FirstOrDefaultAsync(report => report.Id == id);

        public Task<List<Report>> GetAllByStatusAsync(ReportStatus status) => Details().Where(report => report.Status == status).OrderByDescending(report => report.CreatedAt).ToListAsync();

        public Task<List<Report>> GetAllByTargetTypeAsync(ReportTargetType targetType) => Details().Where(report => report.TargetType == targetType).OrderByDescending(report => report.CreatedAt).ToListAsync();

        public Task<List<Report>> GetAllByReporterUserIdAsync(int reporterUserId) => Details().Where(report => report.ReporterUserId == reporterUserId).OrderByDescending(report => report.CreatedAt).ToListAsync();

        public Task<List<Report>> GetReportsForTargetAsync(ReportTargetType targetType, int targetId, CancellationToken cancellationToken = default) => _context.Reports.AsNoTracking().Include(report => report.ReporterUser).Include(report => report.ReviewedByUser).Include(report => report.ModerationActions).ThenInclude(action => action.ModeratorUser).Where(report => report.TargetType == targetType && ((targetType == ReportTargetType.Post && report.TargetPostId == targetId) || (targetType == ReportTargetType.User && report.TargetUserId == targetId) || (targetType == ReportTargetType.Comment && report.TargetCommentId == targetId))).OrderByDescending(report => report.CreatedAt).ToListAsync(cancellationToken);

        public Task<bool> HasPendingPostReportAsync(int reporterUserId, int postId) => HasOpenReportAsync(reporterUserId, ReportTargetType.Post, postId);

        public Task<bool> HasPendingUserReportAsync(int reporterUserId, int targetUserId) => HasOpenReportAsync(reporterUserId, ReportTargetType.User, targetUserId);

        public Task<bool> HasPendingCommentReportAsync(int reporterUserId, int commentId, CancellationToken cancellationToken = default) => _context.Reports.AnyAsync(report => report.ReporterUserId == reporterUserId && report.TargetType == ReportTargetType.Comment && report.TargetCommentId == commentId && (report.Status == ReportStatus.Pending || report.Status == ReportStatus.InReview), cancellationToken);

        public async Task<(List<Report> Reports, int TotalCount)> GetFilteredReportGroupsAsync(ReportStatus? status, ReportTargetType? targetType, ReportReason? reason, DateTime? fromDate, DateTime? toDate, int page, int size, CancellationToken cancellationToken = default)
        {
            IQueryable<Report> query = _context.Reports.AsNoTracking();

            if (status.HasValue)
            {
                query = query.Where(report => report.Status == status.Value);
            }

            if (targetType.HasValue)
            {
                query = query.Where(report => report.TargetType == targetType.Value);
            }

            if (reason.HasValue)
            {
                query = query.Where(report => report.Reason == reason.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(report => report.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(report => report.CreatedAt <= toDate.Value);
            }

            var groupedQuery = query.GroupBy(report => new { report.TargetType, TargetId = report.TargetType == ReportTargetType.Post ? report.TargetPostId : report.TargetType == ReportTargetType.User ? report.TargetUserId : report.TargetCommentId }).Select(group => new { group.Key.TargetType, group.Key.TargetId, LastReportDate = group.Max(report => report.CreatedAt) });
            var totalCount = await groupedQuery.CountAsync(cancellationToken);
            var pageKeys = await groupedQuery.OrderByDescending(group => group.LastReportDate).Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
            var postIds = pageKeys.Where(key => key.TargetType == ReportTargetType.Post && key.TargetId.HasValue).Select(key => key.TargetId!.Value).ToList();
            var userIds = pageKeys.Where(key => key.TargetType == ReportTargetType.User && key.TargetId.HasValue).Select(key => key.TargetId!.Value).ToList();
            var commentIds = pageKeys.Where(key => key.TargetType == ReportTargetType.Comment && key.TargetId.HasValue).Select(key => key.TargetId!.Value).ToList();

            if (postIds.Count == 0 && userIds.Count == 0 && commentIds.Count == 0)
            {
                return (new List<Report>(), totalCount);
            }

            var reports = await query.Include(report => report.ReporterUser).Include(report => report.TargetPost).Include(report => report.TargetUser).Include(report => report.TargetComment).ThenInclude(comment => comment!.User).Include(report => report.ReviewedByUser).Where(report => (report.TargetType == ReportTargetType.Post && report.TargetPostId.HasValue && postIds.Contains(report.TargetPostId.Value)) || (report.TargetType == ReportTargetType.User && report.TargetUserId.HasValue && userIds.Contains(report.TargetUserId.Value)) || (report.TargetType == ReportTargetType.Comment && report.TargetCommentId.HasValue && commentIds.Contains(report.TargetCommentId.Value))).OrderByDescending(report => report.CreatedAt).ToListAsync(cancellationToken);
            return (reports, totalCount);
        }

        public Task<List<Report>> GetOpenReportsForTargetAsync(ReportTargetType targetType, int targetId, CancellationToken cancellationToken = default) => _context.Reports.Where(report => report.TargetType == targetType && (report.Status == ReportStatus.Pending || report.Status == ReportStatus.InReview) && ((targetType == ReportTargetType.Post && report.TargetPostId == targetId) || (targetType == ReportTargetType.User && report.TargetUserId == targetId) || (targetType == ReportTargetType.Comment && report.TargetCommentId == targetId))).ToListAsync(cancellationToken);

        public Task<int> CountRecentReportsByUserAsync(int reporterUserId, DateTime since, CancellationToken cancellationToken = default) => _context.Reports.CountAsync(report => report.ReporterUserId == reporterUserId && report.CreatedAt >= since, cancellationToken);

        private Task<bool> HasOpenReportAsync(int reporterUserId, ReportTargetType targetType, int targetId) => _context.Reports.AnyAsync(report => report.ReporterUserId == reporterUserId && report.TargetType == targetType && ((targetType == ReportTargetType.Post && report.TargetPostId == targetId) || (targetType == ReportTargetType.User && report.TargetUserId == targetId)) && (report.Status == ReportStatus.Pending || report.Status == ReportStatus.InReview));

        private IQueryable<Report> Details() => _context.Reports.Include(report => report.ReporterUser).Include(report => report.TargetPost).Include(report => report.TargetUser).Include(report => report.TargetComment).ThenInclude(comment => comment!.User).Include(report => report.ReviewedByUser);
    }
}
