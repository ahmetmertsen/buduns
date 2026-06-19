using blogapp_server.Application.Features.Report.Commands.CreatePostReport;
using blogapp_server.Application.Features.Report.Commands.ReviewReport;
using blogapp_server.Domain.Enums;

namespace blogapp_server.UnitTests.Validators;

public class ReportValidatorTests
{
    [Fact]
    public async Task ReviewReport_InReviewWithoutAction_ShouldSucceed()
    {
        var result = await new ReviewReportCommandValidator().ValidateAsync(new ReviewReportCommand
        {
            ReportId = 1,
            Status = ReportStatus.InReview,
            ActionType = ModerationActionType.None,
            ReviewNote = "İnceleme başladı"
        });

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ReviewReport_ActionTakenWithoutAction_ShouldFail()
    {
        var result = await new ReviewReportCommandValidator().ValidateAsync(new ReviewReportCommand
        {
            ReportId = 1,
            Status = ReportStatus.ResolvedActionTaken,
            ActionType = ModerationActionType.None,
            ReviewNote = "İhlal bulundu"
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ReviewReportCommand.ActionType));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(366)]
    public async Task ReviewReport_SuspendUserWithInvalidDuration_ShouldFail(int? suspensionDays)
    {
        var result = await new ReviewReportCommandValidator().ValidateAsync(new ReviewReportCommand
        {
            ReportId = 1,
            Status = ReportStatus.ResolvedActionTaken,
            ActionType = ModerationActionType.SuspendUser,
            SuspensionDays = suspensionDays,
            ReviewNote = "Geçici uzaklaştırma"
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ReviewReportCommand.SuspensionDays));
    }

    [Fact]
    public async Task ReviewReport_EmptyReviewNote_ShouldFail()
    {
        var result = await new ReviewReportCommandValidator().ValidateAsync(new ReviewReportCommand
        {
            ReportId = 1,
            Status = ReportStatus.InReview,
            ActionType = ModerationActionType.None,
            ReviewNote = ""
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ReviewReportCommand.ReviewNote));
    }

    [Fact]
    public async Task CreatePostReport_OtherReasonWithoutDescription_ShouldFail()
    {
        var result = await new CreatePostReportCommandValidator().ValidateAsync(new CreatePostReportCommand
        {
            PostId = 1,
            Reason = ReportReason.Other,
            Description = ""
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreatePostReportCommand.Description));
    }

    [Fact]
    public async Task CreatePostReport_ValidRequest_ShouldSucceed()
    {
        var result = await new CreatePostReportCommandValidator().ValidateAsync(new CreatePostReportCommand
        {
            PostId = 1,
            Reason = ReportReason.Spam,
            Description = "Tekrarlayan içerik"
        });

        Assert.True(result.IsValid);
    }
}
