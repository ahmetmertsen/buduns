namespace buduns_server.Application.Abstractions.Services
{
    public interface IClientContext
    {
        string? DeviceName { get; }
        string? UserAgent { get; }
        string? IpAddress { get; }
    }
}
