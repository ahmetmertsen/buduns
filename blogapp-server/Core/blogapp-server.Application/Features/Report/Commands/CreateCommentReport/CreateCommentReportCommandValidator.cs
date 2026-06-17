using blogapp_server.Domain.Enums;
using FluentValidation;

namespace blogapp_server.Application.Features.Report.Commands.CreateCommentReport
{
    public class CreateCommentReportCommandValidator : AbstractValidator<CreateCommentReportCommand>
    {
        public CreateCommentReportCommandValidator()
        {
            RuleFor(x => x.CommentId).GreaterThan(0).WithMessage("Yorum Id 0'dan büyük olmalıdır.");
            RuleFor(x => x.Reason).IsInEnum().WithMessage("Şikayet nedeni geçersiz.");
            RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Şikayet açıklaması en fazla 1000 karakter olabilir.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Diğer sebebi seçildiğinde açıklama zorunludur.").When(x => x.Reason == ReportReason.Other);
        }
    }
}
