namespace buduns_server.Application.Common.Interfaces
{
    public interface ICurrentSessionRequest
    {
        Guid CurrentSessionId { get; set; }
    }
}
