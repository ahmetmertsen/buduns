using buduns_server.Domain.Enums;

namespace buduns_server.Application.Dtos
{
    public class RelatedReportDto
    {
        public int Id { get; set; }
        public int ReporterUserId { get; set; }
        public string? ReporterUserName { get; set; }
        public string? ReporterFullName { get; set; }
        public ReportReason Reason { get; set; }
        public string? Description { get; set; }
        public ReportStatus Status { get; set; }
        public int? ReviewedByUserId { get; set; }
        public string? ReviewedByUserName { get; set; }
        public string? ReviewNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedDate { get; set; }
    }
}
