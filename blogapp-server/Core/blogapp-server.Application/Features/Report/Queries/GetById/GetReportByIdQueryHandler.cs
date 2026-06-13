using AutoMapper;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using blogapp_server.Domain.Enums;
using MediatR;

namespace blogapp_server.Application.Features.Report.Queries.GetById
{
    public class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, ReportDetailDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetReportByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReportDetailDto> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.ReportRepository.GetByIdWithDetailsAsync(request.ReportId);
            if (report == null)
            {
                throw new NotFoundException("Şikayet bulunamadı.");
            }

            var targetId = report.TargetType == ReportTargetType.Post ? report.TargetPostId : report.TargetType == ReportTargetType.User ? report.TargetUserId : report.TargetCommentId;
            if (!targetId.HasValue)
            {
                throw new BadRequestException("Şikayet hedefi bulunamadı.");
            }

            var relatedReports = await _unitOfWork.ReportRepository.GetReportsForTargetAsync(report.TargetType, targetId.Value, cancellationToken);

            var response = _mapper.Map<ReportDetailDto>(report);
            response.ReportCount = relatedReports.Count;
            response.RelatedReports = _mapper.Map<List<RelatedReportDto>>(relatedReports);
            response.ModerationActions = _mapper.Map<List<ModerationActionDto>>(relatedReports
                    .SelectMany(relatedReport => relatedReport.ModerationActions)
                    .OrderByDescending(action => action.CreatedAt));

            return response;
        }
    }
}
