using blogapp_server.Domain.Enums;

namespace blogapp_server.Application.Common.Helpers
{
    public static class ReportPriorityHelper
    {
        public static ReportPriority GetPriority(ReportReason reason) => reason switch
        {
            ReportReason.SelfHarm or ReportReason.Threat or ReportReason.PersonalInformation => ReportPriority.Critical,
            ReportReason.HateSpeech or ReportReason.Violence or ReportReason.SexualContent or ReportReason.Harassment => ReportPriority.High,
            ReportReason.Misinformation or ReportReason.Scam or ReportReason.Impersonation => ReportPriority.Medium,
            _ => ReportPriority.Low
        };

        public static ReportPriority GetHighestPriority(IEnumerable<ReportReason> reasons)
        {
            return reasons.Select(GetPriority).DefaultIfEmpty(ReportPriority.Low).Max();
        }
    }
}
