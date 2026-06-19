using blogapp_server.Application.Abstractions.Services;
using MediatR;

namespace blogapp_server.Application.Features.Auth.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutCommandResponse>
    {
        private readonly IAuthSessionService _authSessionService;

        public LogoutCommandHandler(IAuthSessionService authSessionService)
        {
            _authSessionService = authSessionService;
        }

        public async Task<LogoutCommandResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _authSessionService.RevokeSessionAsync(request.UserId, request.CurrentSessionId, "User logout", cancellationToken);

            return new LogoutCommandResponse(true, "Oturum başarıyla kapatıldı.");
        }
    }
}
