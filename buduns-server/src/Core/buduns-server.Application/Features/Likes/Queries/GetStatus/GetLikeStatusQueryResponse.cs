namespace buduns_server.Application.Features.Likes.Queries.GetStatus
{
    public record GetLikeStatusQueryResponse(bool IsLiked, int? LikeId);
}
