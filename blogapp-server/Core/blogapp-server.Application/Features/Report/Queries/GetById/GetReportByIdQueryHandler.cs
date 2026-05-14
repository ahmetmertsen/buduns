using blogapp_server.Application.Dtos;
using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Report.Queries.GetById
{
    public class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, ReportDetailDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetReportByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReportDetailDto> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.ReportRepository.GetByIdWithDetailsAsync(request.ReportId);
            if (report == null)
            {
                throw new NotFoundException("Þikayet bulunamadý.");
            }

            return new ReportDetailDto
            {
                Id = report.Id,

                ReporterUserId = report.ReporterUserId,
                ReporterUserName = report.ReporterUser?.UserName,
                ReporterFullName = report.ReporterUser?.FullName,
                ReporterEmail = report.ReporterUser?.Email,

                TargetType = report.TargetType,

                TargetPostId = report.TargetPostId,
                TargetPostTitle = report.TargetPost?.Title,
                TargetPostContent = report.TargetPost?.Content,

                TargetUserId = report.TargetUserId,
                TargetUserName = report.TargetUser?.UserName,
                TargetUserFullName = report.TargetUser?.FullName,
                TargetUserEmail = report.TargetUser?.Email,

                Reason = report.Reason,
                Description = report.Description,

                Status = report.Status,

                ReviewedByUserId = report.ReviewedByUserId,
                ReviewedByUserName = report.ReviewedByUser?.UserName,

                CreatedDate = report.CreatedAt,
                ReviewedDate = report.ReviewedDate,
                ReviewNote = report.ReviewNote
            };
        }
    }
}
