using FluentValidation;

namespace blogapp_server.Application.Features.Report.Queries.GetReports
{
    public class GetReportsRequestValidator : AbstractValidator<GetReportsRequest>
    {
        public GetReportsRequestValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Rapor durumu geçersiz.")
                .When(x => x.Status.HasValue);

            RuleFor(x => x.TargetType)
                .IsInEnum().WithMessage("Rapor hedef tipi geçersiz.")
                .When(x => x.TargetType.HasValue);

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page 1 veya daha büyük olmalıdır.");

            RuleFor(x => x.Size)
                .InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
        }
    }
}
