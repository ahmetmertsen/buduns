namespace blogapp_server.Application.Common.Helpers
{
    public static class ReportSnapshotHelper
    {
        private const int MaxSnapshotLength = 1000;

        public static string? CreateContentSnapshot(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            var normalizedContent = content.Trim();
            return normalizedContent.Length <= MaxSnapshotLength ? normalizedContent : normalizedContent[..MaxSnapshotLength];
        }
    }
}
