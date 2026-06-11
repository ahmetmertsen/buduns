namespace blogapp_server.Application.Exceptions
{
    public class InvalidRefreshTokenException : ApplicationException
    {
        public InvalidRefreshTokenException(string message)
            : base(message, 401, "INVALID_REFRESH_TOKEN")
        {
        }
    }
}
