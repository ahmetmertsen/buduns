using blogapp_server.Application.Repositories;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
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
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly BlogAppDbContext _context;
        public ReportRepository(BlogAppDbContext context) : base(context) { _context = context; }

        public override async Task<List<Report>> GetAllAsync() => await _context.Reports.ToListAsync();

        public override async Task<Report?> GetByIdAsync(int id) => await _context.Reports
            .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<List<Report>> GetAllWithDetailsAsync() => await _context.Reports
            .Include(r => r.ReporterUser)
            .Include(r => r.TargetPost)
            .Include(r => r.TargetUser)
            .Include(r => r.ReviewedByUser)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        public async Task<Report?> GetByIdWithDetailsAsync(int id) => await _context.Reports
            .Include(r => r.ReporterUser)
            .Include(r => r.TargetPost)
            .Include(r => r.TargetUser)
            .Include(r => r.ReviewedByUser)
            .Include(r => r.ModerationActions)
            .ThenInclude(action => action.ModeratorUser)
            .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<List<Report>> GetAllByStatusAsync(ReportStatus status) => await _context.Reports
            .Include(r => r.ReporterUser)
            .Include(r => r.TargetPost)
            .Include(r => r.TargetUser)
            .Where(r => r.Status == status)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        public async Task<List<Report>> GetAllByTargetTypeAsync(ReportTargetType targetType) => await _context.Reports
            .Include(r => r.ReporterUser)
            .Include(r => r.TargetPost)
            .Include(r => r.TargetUser)
            .Where(r => r.TargetType == targetType)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        public async Task<List<Report>> GetAllByReporterUserIdAsync(int reporterUserId) => await _context.Reports
            .Include(r => r.TargetPost)
            .Include(r => r.TargetUser)
            .Where(r => r.ReporterUserId == reporterUserId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        public Task<List<Report>> GetReportsForTargetAsync(ReportTargetType targetType, int targetId, CancellationToken cancellationToken = default) => _context.Reports
            .AsNoTracking()
            .Include(report => report.ReporterUser)
            .Include(report => report.ReviewedByUser)
            .Include(report => report.ModerationActions)
                .ThenInclude(action => action.ModeratorUser)
            .Where(report => report.TargetType == targetType &&
                    ((targetType == ReportTargetType.Post && report.TargetPostId == targetId) || (targetType == ReportTargetType.User && report.TargetUserId == targetId)))
            .OrderByDescending(report => report.CreatedAt)
            .ToListAsync(cancellationToken);


        public async Task<bool> HasPendingPostReportAsync(int reporterUserId, int postId) => await _context.Reports.AnyAsync(r =>
                r.ReporterUserId == reporterUserId &&
                r.TargetType == ReportTargetType.Post &&
                r.TargetPostId == postId &&
                (r.Status == ReportStatus.Pending || r.Status == ReportStatus.InReview));

        public async Task<bool> HasPendingUserReportAsync(int reporterUserId, int targetUserId) => await _context.Reports.AnyAsync(r =>
                r.ReporterUserId == reporterUserId &&
                r.TargetType == ReportTargetType.User &&
                r.TargetUserId == targetUserId &&
                (r.Status == ReportStatus.Pending || r.Status == ReportStatus.InReview));


        public async Task<(List<Report> Reports, int TotalCount)> GetFilteredReportGroupsAsync(ReportStatus? status,ReportTargetType? targetType, ReportReason? reason, DateTime? fromDate, DateTime? toDate, int page, int size, CancellationToken cancellationToken = default)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 20 : size;

            IQueryable<Report> query = _context.Reports.AsNoTracking();

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            if (targetType.HasValue)
            {
                query = query.Where(r => r.TargetType == targetType.Value);
            }

            if (reason.HasValue)
            {
                query = query.Where(r => r.Reason == reason.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(r => r.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(r => r.CreatedAt <= toDate.Value);
            }

            var groupedQuery = query
                .GroupBy(r => new
                {
                    r.TargetType,
                    TargetId = r.TargetType == ReportTargetType.Post ? r.TargetPostId : r.TargetUserId
                })
                .Select(group => new
                {
                    group.Key.TargetType,
                    group.Key.TargetId,
                    LastReportDate = group.Max(report => report.CreatedAt)
                });

            var totalCount = await groupedQuery.CountAsync(cancellationToken);
            var pageKeys = await groupedQuery
                .OrderByDescending(group => group.LastReportDate)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            var postIds = pageKeys
                .Where(key => key.TargetType == ReportTargetType.Post && key.TargetId.HasValue)
                .Select(key => key.TargetId!.Value)
                .ToList();
            var userIds = pageKeys
                .Where(key => key.TargetType == ReportTargetType.User && key.TargetId.HasValue)
                .Select(key => key.TargetId!.Value)
                .ToList();

            if (postIds.Count == 0 && userIds.Count == 0)
            {
                return (new List<Report>(), totalCount);
            }

            var reports = await query
                .Include(r => r.ReporterUser)
                .Include(r => r.TargetPost)
                .Include(r => r.TargetUser)
                .Include(r => r.ReviewedByUser)
                .Where(r =>
                    (r.TargetType == ReportTargetType.Post && r.TargetPostId.HasValue && postIds.Contains(r.TargetPostId.Value)) ||
                    (r.TargetType == ReportTargetType.User && r.TargetUserId.HasValue && userIds.Contains(r.TargetUserId.Value)))
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);

            return (reports, totalCount);
        }

        public Task<List<Report>> GetOpenReportsForTargetAsync(ReportTargetType targetType, int targetId, CancellationToken cancellationToken = default) => _context.Reports
                .Where(report =>
                    report.TargetType == targetType &&
                    (report.Status == ReportStatus.Pending || report.Status == ReportStatus.InReview) &&
                    ((targetType == ReportTargetType.Post && report.TargetPostId == targetId) ||
                     (targetType == ReportTargetType.User && report.TargetUserId == targetId)))
                .ToListAsync(cancellationToken);

        public Task<int> CountRecentReportsByUserAsync(int reporterUserId, DateTime since, CancellationToken cancellationToken = default) => _context.Reports.CountAsync(
                report => report.ReporterUserId == reporterUserId && report.CreatedAt >= since,
                cancellationToken);
    }
}
