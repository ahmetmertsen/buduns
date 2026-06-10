namespace blogapp_server.Domain.Enums
{
    public enum ModerationActionType
    {
        None = 0,
        HidePost = 1,
        DeletePost = 2,
        WarnUser = 3,
        SuspendUser = 4,
        BanUser = 5
    }
}
