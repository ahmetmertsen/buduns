using buduns_server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Dtos
{
    public class ReportListDto
    {
        public int Id { get; set; }
        public int ReporterUserId { get; set; }
        public string? ReporterUserName { get; set; }
        public string? ReporterFullName { get; set; }

        public ReportTargetType TargetType { get; set; }
        public ReportPriority Priority { get; set; }

        public int? TargetPostId { get; set; }
        public string? TargetPostContentPreview { get; set; }

        public int? TargetUserId { get; set; }
        public string? TargetUserName { get; set; }
        public string? TargetUserFullName { get; set; }

        public int? TargetCommentId { get; set; }
        public string? TargetCommentContentPreview { get; set; }

        public int? TargetOwnerUserId { get; set; }
        public string? TargetOwnerUserName { get; set; }
        public string? TargetOwnerFullName { get; set; }

        public ReportReason Reason { get; set; }
        public Dictionary<ReportReason, int> ReasonCounts { get; set; } = new();
        public ReportStatus Status { get; set; }

        public int ReportCount { get; set; }
        public DateTime FirstReportDate { get; set; }
        public DateTime LastReportDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
