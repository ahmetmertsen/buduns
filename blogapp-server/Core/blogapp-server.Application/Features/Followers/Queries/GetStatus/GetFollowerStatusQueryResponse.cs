namespace blogapp_server.Application.Features.Followers.Queries.GetStatus
{
    public record GetFollowerStatusQueryResponse(bool IsFollowing, int? FollowId);
}
