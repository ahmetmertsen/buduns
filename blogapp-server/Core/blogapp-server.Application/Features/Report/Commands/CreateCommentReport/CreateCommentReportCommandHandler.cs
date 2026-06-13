using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace blogapp_server.Application.Features.Report.Commands.CreateCommentReport
{
    public class CreateCommentReportCommandHandler : IRequestHandler<CreateCommentReportCommand, CreateCommentReportCommandResponse>
    {
        private const int DailyReportLimit = 10;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCommentReportCommandHandler> _logger;

        public CreateCommentReportCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCommentReportCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CreateCommentReportCommandResponse> Handle(CreateCommentReportCommand request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.CommentRepository.GetVisibleByIdAsync(request.CommentId, cancellationToken);
            if (comment == null)
            {
                throw new NotFoundException("Şikayet edilecek yorum bulunamadı.");
            }

            if (comment.UserId == request.UserId)
            {
                throw new BadRequestException("Kendi yorumunuzu şikayet edemezsiniz.");
            }

            var recentReportCount = await _unitOfWork.ReportRepository.CountRecentReportsByUserAsync(request.UserId, DateTime.UtcNow.AddHours(-24), cancellationToken);
            if (recentReportCount >= DailyReportLimit)
            {
                throw new TooManyRequestsException("24 saat içinde en fazla 10 şikayet oluşturabilirsiniz.");
            }

            if (await _unitOfWork.ReportRepository.HasPendingCommentReportAsync(request.UserId, request.CommentId, cancellationToken))
            {
                throw new BadRequestException("Bu yorum için zaten bekleyen bir şikayetiniz var.");
            }

            var report = new Domain.Entities.Report
            {
                ReporterUserId = request.UserId,
                TargetType = ReportTargetType.Comment,
                TargetPostId = null,
                TargetUserId = null,
                TargetCommentId = request.CommentId,
                Reason = request.Reason,
                Description = request.Description?.Trim(),
                Status = ReportStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                isActive = true,
                isDeleted = false
            };

            await _unitOfWork.ReportRepository.AddAsync(report);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Comment report created. ReportId: {ReportId}, ReporterUserId: {ReporterUserId}, TargetCommentId: {TargetCommentId}, Reason: {Reason}", report.Id, request.UserId, request.CommentId, request.Reason);
            return new CreateCommentReportCommandResponse(true, "Şikayetiniz başarıyla alındı.");
        }
    }
}
