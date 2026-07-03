namespace buduns_server.Application.Exceptions
{
    public class TooManyRequestsException : ApplicationException
    {
        public TooManyRequestsException(string message) : base(message, 429, "TOO_MANY_REQUESTS")
        {
        }
    }
}
