using buduns_server.Domain.Enums;
using FluentValidation;

namespace buduns_server.Application.Features.Report.Commands.ReviewReport
{
    public class ReviewReportCommandValidator : AbstractValidator<ReviewReportCommand>
    {
        public ReviewReportCommandValidator()
        {
            RuleFor(x => x.ReportId)
                .GreaterThan(0).WithMessage("Report Id 0'dan büyük olmalıdır.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Rapor durumu geçersiz.")
                .NotEqual(ReportStatus.Pending).WithMessage("Rapor tekrar beklemede durumuna alınamaz.");

            RuleFor(x => x.ActionType)
                .IsInEnum().WithMessage("Moderasyon aksiyonu geçersiz.");

            RuleFor(x => x.ActionType)
                .Equal(ModerationActionType.None)
                .WithMessage("İnceleme veya ihlal yok kararında moderasyon aksiyonu uygulanamaz.")
                .When(x => x.Status == ReportStatus.InReview || x.Status == ReportStatus.ResolvedNoViolation);

            RuleFor(x => x.ActionType)
                .NotEqual(ModerationActionType.None)
                .WithMessage("Aksiyon alındı kararında bir moderasyon aksiyonu seçilmelidir.")
                .When(x => x.Status == ReportStatus.ResolvedActionTaken);

            RuleFor(x => x.SuspensionDays)
                .NotNull().WithMessage("Hesap askıya alma süresi zorunludur.")
                .InclusiveBetween(1, 365).WithMessage("Askıya alma süresi 1 ile 365 gün arasında olmalıdır.")
                .When(x => x.ActionType == ModerationActionType.SuspendUser);

            RuleFor(x => x.ReviewNote)
                .NotEmpty().WithMessage("İnceleme notu zorunludur.")
                .MaximumLength(1000).WithMessage("İnceleme notu en fazla 1000 karakter olabilir.");
        }
    }
}
