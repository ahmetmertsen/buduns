using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Entities.Identity;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ReportEntity = blogapp_server.Domain.Entities.Report;

namespace blogapp_server.Application.Features.Report.Commands.ReviewReport
{
    public class ReviewReportCommandHandler : IRequestHandler<ReviewReportCommand, ReviewReportCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ReviewReportCommandHandler> _logger;

        public ReviewReportCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, ILogger<ReviewReportCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ReviewReportCommandResponse> Handle(ReviewReportCommand request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.ReportRepository.GetByIdWithDetailsAsync(request.ReportId);
            if (report == null)
            {
                throw new NotFoundException("Şikayet bulunamadı.");
            }

            if (report.Status != ReportStatus.Pending && report.Status != ReportStatus.InReview)
            {
                throw new BadRequestException("Bu şikayet zaten sonuçlandırılmış.");
            }

            if (request.Status == ReportStatus.InReview)
            {
                report.Status = ReportStatus.InReview;
                report.ReviewNote = request.ReviewNote?.Trim();
                report.ReviewedByUserId = request.UserId;
                report.UpdateAt = DateTime.UtcNow;
                _unitOfWork.ReportRepository.Update(report);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Report review started. ReportId: {ReportId}, ModeratorUserId: {ModeratorUserId}",
                    report.Id,
                    request.UserId);

                return new ReviewReportCommandResponse(true, "Şikayet incelemeye alındı.");
            }

            ValidateActionForTarget(report, request.ActionType);

            var actionTargetUserId = ResolveActionTargetUserId(report, request.ActionType);
            if (actionTargetUserId == request.UserId &&
                (request.ActionType == ModerationActionType.WarnUser ||
                 request.ActionType == ModerationActionType.SuspendUser ||
                 request.ActionType == ModerationActionType.BanUser))
            {
                throw new BadRequestException("Moderatör kendi hesabına moderasyon aksiyonu uygulayamaz.");
            }

            var actionExpiresAt = await ApplyModerationActionAsync(report, request);
            var targetId = GetTargetId(report);
            var openReports = await _unitOfWork.ReportRepository.GetOpenReportsForTargetAsync(report.TargetType, targetId, cancellationToken);
            var reviewedAt = DateTime.UtcNow;

            foreach (var openReport in openReports)
            {
                openReport.Status = request.Status;
                openReport.ReviewNote = request.ReviewNote?.Trim();
                openReport.ReviewedByUserId = request.UserId;
                openReport.ReviewedDate = reviewedAt;
                openReport.UpdateAt = reviewedAt;
                _unitOfWork.ReportRepository.Update(openReport);
            }

            var moderationAction = new ModerationAction
            {
                ReportId = report.Id,
                ModeratorUserId = request.UserId,
                ActionType = request.ActionType,
                TargetType = report.TargetType,
                TargetPostId = report.TargetPostId,
                TargetUserId = ResolveActionTargetUserId(report, request.ActionType),
                Note = request.ReviewNote?.Trim(),
                ExpiresAt = actionExpiresAt,
                CreatedAt = reviewedAt,
                UpdateAt = reviewedAt,
                isActive = true,
                isDeleted = false
            };
            await _unitOfWork.ModerationActionRepository.AddAsync(moderationAction);

            await AddReporterNotificationsAsync(openReports);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Report resolved. ReportId: {ReportId}, ModeratorUserId: {ModeratorUserId}, Status: {Status}, ActionType: {ActionType}, TargetType: {TargetType}, TargetId: {TargetId}, ResolvedReportCount: {ResolvedReportCount}",
                report.Id,
                request.UserId,
                request.Status,
                request.ActionType,
                report.TargetType,
                targetId,
                openReports.Count);

            return new ReviewReportCommandResponse(true, "Şikayet ve aynı hedefe ait açık şikayetler sonuçlandırıldı.");
        }

        private async Task<DateTime?> ApplyModerationActionAsync(ReportEntity report, ReviewReportCommand request)
        {
            switch (request.ActionType)
            {
                case ModerationActionType.None:
                    return null;

                case ModerationActionType.HidePost:
                    var postToHide = GetTargetPost(report);
                    postToHide.Status = PostStatus.HiddenByModerator;
                    postToHide.isPublished = false;
                    postToHide.isActive = false;
                    postToHide.UpdateAt = DateTime.UtcNow;
                    _unitOfWork.PostRepository.Update(postToHide);
                    await AddTargetNotificationAsync(postToHide.UserId, NotificationType.POST_HIDDEN, "Paylaşımınız moderasyon kararıyla gizlendi.");
                    return null;

                case ModerationActionType.DeletePost:
                    var postToDelete = GetTargetPost(report);
                    postToDelete.Status = PostStatus.DeletedByModerator;
                    postToDelete.isPublished = false;
                    postToDelete.isActive = false;
                    postToDelete.isDeleted = true;
                    postToDelete.UpdateAt = DateTime.UtcNow;
                    _unitOfWork.PostRepository.Update(postToDelete);
                    await AddTargetNotificationAsync(postToDelete.UserId, NotificationType.POST_REMOVED, "Paylaşımınız moderasyon kararıyla kaldırıldı.");
                    return null;

                case ModerationActionType.WarnUser:
                    var warnedUserId = ResolveActionTargetUserId(report, request.ActionType)
                        ?? throw new BadRequestException("Uyarılacak kullanıcı bulunamadı.");
                    await AddTargetNotificationAsync(warnedUserId, NotificationType.MODERATION_WARNING, "Hesabınız bir topluluk kuralı ihlali nedeniyle uyarıldı.");
                    return null;

                case ModerationActionType.SuspendUser:
                    var suspendedUser = await GetActionTargetUserAsync(report);
                    var suspensionEnd = DateTime.UtcNow.AddDays(request.SuspensionDays!.Value);
                    suspendedUser.Status = UserStatus.Suspended;
                    suspendedUser.SuspendedUntil = suspensionEnd;
                    suspendedUser.RefreshToken = null;
                    suspendedUser.RefreshTokenEndDate = null;
                    suspendedUser.LockoutEnabled = true;
                    suspendedUser.LockoutEnd = new DateTimeOffset(suspensionEnd);
                    await AddTargetNotificationAsync(suspendedUser.Id, NotificationType.ACCOUNT_SUSPENDED, $"Hesabınız {request.SuspensionDays.Value} gün süreyle askıya alındı.");
                    return suspensionEnd;

                case ModerationActionType.BanUser:
                    var bannedUser = await GetActionTargetUserAsync(report);
                    bannedUser.Status = UserStatus.Banned;
                    bannedUser.SuspendedUntil = null;
                    bannedUser.RefreshToken = null;
                    bannedUser.RefreshTokenEndDate = null;
                    bannedUser.LockoutEnabled = true;
                    bannedUser.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
                    await AddTargetNotificationAsync(bannedUser.Id, NotificationType.ACCOUNT_BANNED, "Hesabınız moderasyon kararıyla kalıcı olarak yasaklandı.");
                    return null;

                default:
                    throw new BadRequestException("Desteklenmeyen moderasyon aksiyonu.");
            }
        }

        private static void ValidateActionForTarget(ReportEntity report, ModerationActionType actionType)
        {
            if ((actionType == ModerationActionType.HidePost || actionType == ModerationActionType.DeletePost) &&
                report.TargetType != ReportTargetType.Post)
            {
                throw new BadRequestException("Post moderasyon aksiyonu yalnızca post şikayetlerine uygulanabilir.");
            }
        }

        private static Post GetTargetPost(ReportEntity report) =>
            report.TargetPost ?? throw new NotFoundException("Şikayet edilen gönderi bulunamadı.");

        private async Task<User> GetActionTargetUserAsync(ReportEntity report)
        {
            var targetUserId = ResolveActionTargetUserId(report, ModerationActionType.WarnUser)
                ?? throw new NotFoundException("Moderasyon uygulanacak kullanıcı bulunamadı.");
            return await _userManager.FindByIdAsync(targetUserId.ToString())
                ?? throw new NotFoundException("Moderasyon uygulanacak kullanıcı bulunamadı.");
        }

        private static int? ResolveActionTargetUserId(ReportEntity report, ModerationActionType actionType)
        {
            if (actionType == ModerationActionType.None ||
                actionType == ModerationActionType.HidePost ||
                actionType == ModerationActionType.DeletePost)
            {
                return report.TargetUserId;
            }

            return report.TargetType == ReportTargetType.User
                ? report.TargetUserId
                : report.TargetPost?.UserId;
        }

        private static int GetTargetId(ReportEntity report) =>
            report.TargetType == ReportTargetType.Post
                ? report.TargetPostId ?? throw new BadRequestException("Şikayet hedefi bulunamadı.")
                : report.TargetUserId ?? throw new BadRequestException("Şikayet hedefi bulunamadı.");

        private async Task AddReporterNotificationsAsync(IEnumerable<ReportEntity> reports)
        {
            foreach (var reporterUserId in reports.Select(report => report.ReporterUserId).Distinct())
            {
                await AddTargetNotificationAsync(reporterUserId, NotificationType.REPORT_RESOLVED, "Oluşturduğunuz bir şikayet moderasyon ekibi tarafından sonuçlandırıldı.");
            }
        }

        private async Task AddTargetNotificationAsync(int userId, NotificationType type, string message)
        {
            await _unitOfWork.NotificationRepository.AddAsync(new Notification
            {
                UserId = userId,
                Type = type,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                isActive = true,
                isDeleted = false
            });
        }
    }
}
