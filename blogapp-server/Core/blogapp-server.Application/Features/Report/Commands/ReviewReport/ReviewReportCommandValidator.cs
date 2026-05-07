using FluentValidation;

namespace blogapp_server.Application.Features.Report.Commands.ReviewReport
{
    public class ReviewReportCommandValidator : AbstractValidator<ReviewReportCommand>
    {
        public ReviewReportCommandValidator()
        {
            RuleFor(x => x.ReportId)
                .GreaterThan(0).WithMessage("Report Id 0'dan büyük olmalıdır.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Rapor durumu geçersiz.");

            RuleFor(x => x.ReviewNote)
                .MaximumLength(1000).WithMessage("İnceleme notu en fazla 1000 karakter olabilir.");
        }
    }
}
