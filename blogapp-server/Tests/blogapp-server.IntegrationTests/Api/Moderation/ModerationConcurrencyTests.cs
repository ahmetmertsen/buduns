using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Features.Report.Commands.ReviewReport;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using blogapp_server.IntegrationTests.Fixtures;
using blogapp_server.IntegrationTests.Helpers;
using blogapp_server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace blogapp_server.IntegrationTests.Api.Moderation;

public sealed class ModerationConcurrencyTests : IntegrationTestBase
{
    public ModerationConcurrencyTests(BlogAppWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Two_moderators_should_not_claim_the_same_report_concurrently()
    {
        var reporter = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "race-reporter", "Race Reporter", RoleConstants.User));
        var target = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "race-target", "Race Target", RoleConstants.User));
        var firstModerator = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "race-admin-one", "Race Admin One", RoleConstants.Admin));
        var secondModerator = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "race-admin-two", "Race Admin Two", RoleConstants.Admin));
        var report = await Factory.ExecuteScopeAsync(async services =>
        {
            var context = services.GetRequiredService<BlogAppDbContext>();
            var entity = new Report { ReporterUserId = reporter.Id, TargetType = ReportTargetType.User, TargetUserId = target.Id, TargetOwnerUserId = target.Id, TargetOwnerUserNameSnapshot = target.UserName, TargetOwnerFullNameSnapshot = target.FullName, Reason = ReportReason.Harassment, Description = "Concurrency test", Status = ReportStatus.Pending, CreatedAt = DateTime.UtcNow, isActive = true, isDeleted = false };
            context.Reports.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        });
        using var firstAuthentication = await Factory.CreateAuthenticatedClientAsync(firstModerator.Id);
        using var secondAuthentication = await Factory.CreateAuthenticatedClientAsync(secondModerator.Id);
        var firstCommand = new ReviewReportCommand { ReportId = report.Id, Status = ReportStatus.InReview, ActionType = ModerationActionType.None, ReviewNote = "First moderator claim" };
        var secondCommand = new ReviewReportCommand { ReportId = report.Id, Status = ReportStatus.InReview, ActionType = ModerationActionType.None, ReviewNote = "Second moderator claim" };
        var responses = await Task.WhenAll(firstAuthentication.Client.PostAsJsonAsync("/api/Report/review", firstCommand), secondAuthentication.Client.PostAsJsonAsync("/api/Report/review", secondCommand));
        responses.Count(response => response.StatusCode == HttpStatusCode.OK).Should().Be(1);
        responses.Count(response => response.StatusCode == HttpStatusCode.BadRequest).Should().Be(1);
        var storedReport = await Factory.ExecuteScopeAsync(async services => await services.GetRequiredService<BlogAppDbContext>().Reports.AsNoTracking().SingleAsync(item => item.Id == report.Id));
        storedReport.Status.Should().Be(ReportStatus.InReview);
        storedReport.ReviewedByUserId.Should().BeOneOf(firstModerator.Id, secondModerator.Id);
    }
}
