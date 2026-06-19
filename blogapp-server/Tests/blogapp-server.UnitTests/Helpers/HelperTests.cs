using blogapp_server.Application.Common.Helpers;
using blogapp_server.Domain.Enums;

namespace blogapp_server.UnitTests.Helpers;

public class HelperTests
{
    [Theory]
    [InlineData("  dotnet   core  ", "dotnet core")]
    [InlineData("backend", "backend")]
    public void NormalizeDisplayName_ShouldTrimAndCollapseSpaces(string input, string expected)
    {
        Assert.Equal(expected, TagNameNormalizer.NormalizeDisplayName(input));
    }

    [Fact]
    public void NormalizeKey_ShouldProduceCaseInsensitiveKey()
    {
        Assert.Equal(TagNameNormalizer.NormalizeKey("DotNet Core"), TagNameNormalizer.NormalizeKey(" dotnet   core "));
    }

    [Theory]
    [InlineData(ReportReason.Threat, ReportPriority.Critical)]
    [InlineData(ReportReason.Harassment, ReportPriority.High)]
    [InlineData(ReportReason.Scam, ReportPriority.Medium)]
    [InlineData(ReportReason.Spam, ReportPriority.Low)]
    public void GetPriority_ShouldMapReason(ReportReason reason, ReportPriority expected)
    {
        Assert.Equal(expected, ReportPriorityHelper.GetPriority(reason));
    }

    [Fact]
    public void GetHighestPriority_ShouldReturnHighestValue()
    {
        var result = ReportPriorityHelper.GetHighestPriority(new[] { ReportReason.Spam, ReportReason.Scam, ReportReason.Threat });

        Assert.Equal(ReportPriority.Critical, result);
    }

    [Fact]
    public void CreateContentSnapshot_BlankContent_ShouldReturnNull()
    {
        Assert.Null(ReportSnapshotHelper.CreateContentSnapshot("   "));
    }

    [Fact]
    public void CreateContentSnapshot_LongContent_ShouldTrimAndTruncate()
    {
        var result = ReportSnapshotHelper.CreateContentSnapshot($"  {new string('x', 1100)}  ");

        Assert.NotNull(result);
        Assert.Equal(1000, result!.Length);
    }
}
