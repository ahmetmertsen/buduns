using FluentValidation;

namespace blogapp_server.Application.Features.Report.Queries.GetById
{
    public class GetReportByIdQueryValidator : AbstractValidator<GetReportByIdQuery>
    {
        public GetReportByIdQueryValidator()
        {
            RuleFor(x => x.ReportId)
                .GreaterThan(0).WithMessage("Report Id 0'dan büyük olmalıdır.");
        }
    }
}
