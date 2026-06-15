namespace blogapp_server.Application.Features.Likes.Commands.Create
{
    public record CreateLikesCommandResponse(bool Succeeded, string Message, int LikeId, bool AlreadyLiked);
}
