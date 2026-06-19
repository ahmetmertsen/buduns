namespace blogapp_server.Application.Features.Followers.Commands.Create
{
    public record CreateFollowersCommandResponse(bool Succeeded, string Message, int FollowId, bool AlreadyFollowing);
}
