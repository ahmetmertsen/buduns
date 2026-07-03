using buduns_server.Application.Common.Consts;
using buduns_server.Application.Features.Bookmarks.Commands.Create;
using buduns_server.Domain.Entities;
using buduns_server.IntegrationTests.Fixtures;
using buduns_server.IntegrationTests.Helpers;
using buduns_server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace buduns_server.IntegrationTests.Infrastructure.Database;

public sealed class UniquenessTests : IntegrationTestBase
{
    public UniquenessTests(BudunsWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Concurrent_like_requests_should_create_one_like()
    {
        var owner = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "like-owner", "Like Owner", RoleConstants.User));
        var actor = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "like-admin", "Like Admin", RoleConstants.Admin));
        var post = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreatePostAsync(services, owner.Id));
        using var authentication = await Factory.CreateAuthenticatedClientAsync(actor.Id);
        var responses = await Task.WhenAll(authentication.Client.PostAsync($"/api/Like/{post.Id}", null), authentication.Client.PostAsync($"/api/Like/{post.Id}", null));
        responses.Should().OnlyContain(response => response.StatusCode == HttpStatusCode.OK);
        var count = await Factory.ExecuteScopeAsync(async services => await services.GetRequiredService<BudunsDbContext>().Likes.CountAsync(like => like.UserId == actor.Id && like.PostId == post.Id));
        count.Should().Be(1);
    }

    [Fact]
    public async Task Concurrent_bookmark_requests_should_create_one_bookmark()
    {
        var owner = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "bookmark-owner", "Bookmark Owner", RoleConstants.User));
        var actor = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "bookmark-admin", "Bookmark Admin", RoleConstants.Admin));
        var post = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreatePostAsync(services, owner.Id));
        using var authentication = await Factory.CreateAuthenticatedClientAsync(actor.Id);
        var command = new CreateBookmarksCommand { PostId = post.Id };
        var responses = await Task.WhenAll(authentication.Client.PostAsJsonAsync("/api/Bookmark", command), authentication.Client.PostAsJsonAsync("/api/Bookmark", command));
        responses.Should().OnlyContain(response => response.StatusCode == HttpStatusCode.OK);
        var count = await Factory.ExecuteScopeAsync(async services => await services.GetRequiredService<BudunsDbContext>().Bookmarks.CountAsync(bookmark => bookmark.UserId == actor.Id && bookmark.PostId == post.Id));
        count.Should().Be(1);
    }

    [Fact]
    public async Task Concurrent_follow_requests_should_create_one_follow()
    {
        var target = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "follow-target", "Follow Target", RoleConstants.User));
        var actor = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "follow-admin", "Follow Admin", RoleConstants.Admin));
        using var authentication = await Factory.CreateAuthenticatedClientAsync(actor.Id);
        var responses = await Task.WhenAll(authentication.Client.PostAsync($"/api/Follower/{target.Id}", null), authentication.Client.PostAsync($"/api/Follower/{target.Id}", null));
        responses.Should().OnlyContain(response => response.StatusCode == HttpStatusCode.OK);
        var count = await Factory.ExecuteScopeAsync(async services => await services.GetRequiredService<BudunsDbContext>().Followers.CountAsync(follow => follow.FollowerId == actor.Id && follow.FollowingId == target.Id));
        count.Should().Be(1);
    }

    [Fact]
    public async Task Database_should_reject_self_follow_even_without_handler()
    {
        var user = await Factory.ExecuteScopeAsync(services => DatabaseSeeder.CreateUserAsync(services, "self-follow", "Self Follow", RoleConstants.User));
        var action = async () => await Factory.ExecuteScopeAsync(async services =>
        {
            var context = services.GetRequiredService<BudunsDbContext>();
            context.Followers.Add(new Follower { FollowerId = user.Id, FollowingId = user.Id, CreatedAt = DateTime.UtcNow, isActive = true });
            await context.SaveChangesAsync();
        });
        await action.Should().ThrowAsync<DbUpdateException>();
    }
}
