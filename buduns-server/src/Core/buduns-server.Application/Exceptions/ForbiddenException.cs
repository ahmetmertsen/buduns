namespace buduns_server.Application.Exceptions
{
    public class ForbiddenException : ApplicationException
    {
        public ForbiddenException(string message)
            : base(message, 403, "FORBIDDEN")
        {
        }
    }
}
