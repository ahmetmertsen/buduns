using blogapp_server.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<Dtos.Token> LoginAsync(string usernameOrEmail, string password, CancellationToken cancellationToken);
        Task<Dtos.Token> RefreshTokenLoginAsync(string refreshToken, CancellationToken cancellationToken);
        Task<ForgotPasswordResponse> ForgotPasswordResetAsync(ForgotPasswordRequest request, CancellationToken cancellationToken);
        Task<MailVerifyResponse> MailVerifyAsync(MailVerifyRequest request, CancellationToken cancellationToken);
        Task<ChangeEmailResponse> ChangeEmailAsync(ChangeEmailRequest request, CancellationToken cancellationToken);
    }
}
