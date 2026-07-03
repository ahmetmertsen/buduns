namespace buduns_server.Domain.Enums
{
    public enum CommentStatus
    {
        Published = 0,
        DeletedByOwner = 1,
        HiddenByModerator = 2,
        DeletedByModerator = 3
    }
}
