using blogapp_server.Application.Features.Comments.Commands.Create;
using blogapp_server.Application.Features.Posts.Commands.Create;
using blogapp_server.Application.Features.Posts.Queries.GetAll;

namespace blogapp_server.UnitTests.Validators;

public class ContentValidatorTests
{
    [Fact]
    public async Task CreatePost_ValidRequest_ShouldSucceed()
    {
        var result = await new CreatePostsCommandValidator().ValidateAsync(new CreatePostsCommand
        {
            Content = "Geçerli içerik",
            TagIds = new List<int> { 1, 2, 3 }
        });

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    public async Task CreatePost_EmptyContent_ShouldFail(string content)
    {
        var result = await new CreatePostsCommandValidator().ValidateAsync(new CreatePostsCommand { Content = content });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreatePostsCommand.Content));
    }

    [Fact]
    public async Task CreatePost_ContentLongerThanLimit_ShouldFail()
    {
        var result = await new CreatePostsCommandValidator().ValidateAsync(new CreatePostsCommand { Content = new string('x', 1001) });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreatePostsCommand.Content));
    }

    [Fact]
    public async Task CreatePost_MoreThanThreeDistinctTags_ShouldFail()
    {
        var result = await new CreatePostsCommandValidator().ValidateAsync(new CreatePostsCommand
        {
            Content = "İçerik",
            TagIds = new List<int> { 1, 2, 3, 4 }
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreatePostsCommand.TagIds));
    }

    [Fact]
    public async Task CreatePost_NonPositiveTagId_ShouldFail()
    {
        var result = await new CreatePostsCommandValidator().ValidateAsync(new CreatePostsCommand
        {
            Content = "İçerik",
            TagIds = new List<int> { 0 }
        });

        Assert.Contains(result.Errors, error => error.PropertyName.StartsWith(nameof(CreatePostsCommand.TagIds)));
    }

    [Fact]
    public async Task CreateComment_ValidRequest_ShouldSucceed()
    {
        var result = await new CreateCommentsCommandValidator().ValidateAsync(new CreateCommentsCommand
        {
            PostId = 1,
            Content = "Yorum"
        });

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task CreateComment_InvalidPostAndContent_ShouldFailBothProperties()
    {
        var result = await new CreateCommentsCommandValidator().ValidateAsync(new CreateCommentsCommand
        {
            PostId = 0,
            Content = ""
        });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateCommentsCommand.PostId));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateCommentsCommand.Content));
    }

    [Fact]
    public async Task GetAllPosts_ValidFilters_ShouldSucceed()
    {
        var result = await new GetAllPostsQueryValidator().ValidateAsync(new GetAllPostsQuery
        {
            Page = 1,
            Size = 20,
            TagId = 1,
            UserId = 2,
            Search = "dotnet",
            SortBy = "popular"
        });

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("newest")]
    [InlineData("random")]
    public async Task GetAllPosts_UnsupportedSort_ShouldFail(string sortBy)
    {
        var result = await new GetAllPostsQueryValidator().ValidateAsync(new GetAllPostsQuery { SortBy = sortBy });

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAllPostsQuery.SortBy));
    }
}
