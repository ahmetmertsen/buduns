using Serilog.Context;

namespace blogapp_server.WebAPI.Middlewares
{
    public class UserNameLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserNameLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var username = context.User?.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : null;

            if (string.IsNullOrWhiteSpace(username))
            {
                await _next(context);
                return;
            }

            using (LogContext.PushProperty("user_name", username))
            {
                await _next(context);
            }
        }
    }
}
