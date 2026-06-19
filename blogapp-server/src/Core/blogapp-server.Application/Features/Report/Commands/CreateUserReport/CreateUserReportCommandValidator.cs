using blogapp_server.Domain.Enums;
using FluentValidation;

namespace blogapp_server.Application.Features.Report.Commands.CreateUserReport
{
    public class CreateUserReportCommandValidator : AbstractValidator<CreateUserReportCommand>
    {
        public CreateUserReportCommandValidator()
        {
            RuleFor(x => x.TargetUserId)
                .GreaterThan(0).WithMessage("Target User Id 0'dan büyük olmalıdır.");

            RuleFor(x => x.Reason)
                .IsInEnum().WithMessage("Rapor sebebi geçersiz.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Diğer sebebi seçildiğinde açıklama zorunludur.")
                .When(x => x.Reason == ReportReason.Other);
        }
    }
}
