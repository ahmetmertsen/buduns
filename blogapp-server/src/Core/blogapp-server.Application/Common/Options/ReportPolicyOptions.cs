namespace blogapp_server.Application.Common.Options
{
    public class ReportPolicyOptions
    {
        public const string SectionName = "ReportPolicy";

        public int DailyReportLimit { get; set; } = 10;
    }
}
