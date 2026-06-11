namespace blogapp_server.Application.Exceptions
{
    public class EmailVerificationRequiredException : ApplicationException
    {
        public EmailVerificationRequiredException(string message)
            : base(message, 403, "EMAIL_VERIFICATION_REQUIRED")
        {
        }
    }
}
