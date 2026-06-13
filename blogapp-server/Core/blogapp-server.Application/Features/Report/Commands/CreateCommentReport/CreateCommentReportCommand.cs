using blogapp_server.Application.Common.Interfaces;
using blogapp_server.Domain.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Report.Commands.CreateCommentReport
{
    public class CreateCommentReportCommand : IRequest<CreateCommentReportCommandResponse>, ICurrentUserRequest
    {
        public int CommentId { get; set; }
        public ReportReason Reason { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
