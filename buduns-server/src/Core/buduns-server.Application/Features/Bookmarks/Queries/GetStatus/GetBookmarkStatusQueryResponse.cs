namespace buduns_server.Application.Features.Bookmarks.Queries.GetStatus
{
    public record GetBookmarkStatusQueryResponse(bool IsBookmarked, int? BookmarkId);
}
