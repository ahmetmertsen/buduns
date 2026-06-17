namespace blogapp_server.Application.Common.Helpers
{
    public static class TagNameNormalizer
    {
        public static string NormalizeDisplayName(string name) => string.Join(" ", name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));

        public static string NormalizeKey(string name) => NormalizeDisplayName(name).ToUpperInvariant();
    }
}
