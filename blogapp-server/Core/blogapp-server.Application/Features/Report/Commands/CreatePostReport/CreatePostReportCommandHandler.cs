using blogapp_server.Application.Exceptions;
using blogapp_server.Application.Repositories;
using blogapp_server.Application.UnitOfWork;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Report.Commands.CreatePostReport
{
    public class CreatePostReportCommandHandler : IRequestHandler<CreatePostReportCommand, CreatePostReportCommandResponse>
    {
        private const int DailyReportLimit = 10;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreatePostReportCommandHandler> _logger;

        public CreatePostReportCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePostReportCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CreatePostReportCommandResponse> Handle(CreatePostReportCommand request, CancellationToken cancellationToken)
        {
            Post? post = await _unitOfWork.PostRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new NotFoundException("Gönderi bulunamadı.");
            }

            if (post.UserId == request.UserId)
            {
                throw new BadRequestException("Kendi gönderinizi şikayet edemezsiniz.");
            }

            if (post.Status != PostStatus.Published || !post.isPublished || post.isDeleted)
            {
                throw new BadRequestException("Bu gönderi şikayet edilmeye uygun değil.");
            }

            var recentReportCount = await _unitOfWork.ReportRepository.CountRecentReportsByUserAsync(request.UserId, DateTime.UtcNow.AddHours(-24), cancellationToken);
            if (recentReportCount >= DailyReportLimit)
            {
                throw new BadRequestException("24 saat içinde en fazla 10 şikayet oluşturabilirsiniz.");
            }
                
            bool alreadyReported = await _unitOfWork.ReportRepository.HasPendingPostReportAsync(request.UserId, request.PostId);
            if (alreadyReported)
            {
                throw new BadRequestException("Bu gönderi için zaten bekleyen bir şikayetiniz var.");
            }
                
            Domain.Entities.Report report = new()
            {
                ReporterUserId = request.UserId,
                TargetType = ReportTargetType.Post,
                TargetPostId = request.PostId,
                TargetUserId = null,
                Reason = request.Reason,
                Description = request.Description?.Trim(),
                Status = ReportStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ReportRepository.AddAsync(report);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Post report created. ReportId: {ReportId}, ReporterUserId: {ReporterUserId}, TargetPostId: {TargetPostId}, Reason: {Reason}",
                report.Id,
                request.UserId,
                request.PostId,
                request.Reason);

            return new CreatePostReportCommandResponse(Succeeded:true, Message:"Şikayetiniz başarıyla alındı.");
        }
    }
}
