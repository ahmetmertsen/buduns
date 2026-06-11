using blogapp_server.Application.Dtos.Auth;
using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Common.CustomAttrributes;
using blogapp_server.Application.Features.Auth.ChangeEmail;
using blogapp_server.Application.Features.Auth.ForgotPassword;
using blogapp_server.Application.Features.Auth.GetSessions;
using blogapp_server.Application.Features.Auth.Login;
using blogapp_server.Application.Features.Auth.Logout;
using blogapp_server.Application.Features.Auth.LogoutAll;
using blogapp_server.Application.Features.Auth.MailVerify;
using blogapp_server.Application.Features.Auth.RefreshTokenLogin;
using blogapp_server.Application.Features.Auth.RevokeSession;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogapp_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediatR;
        public AuthController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("refreshTokenLogin")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPasswordReset([FromBody] ForgotPasswordCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Auth, ActionType = ActionType.Writing, Definition = "Send Mail Verify")]
        [HttpPost]
        [Route("mailVerify")]
        public async Task<IActionResult> MailVerify()
        {
            var response = await _mediatR.Send(new MailVerifyCommand());
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Auth, ActionType = ActionType.Updating, Definition = "Change Email")]
        [HttpPost]
        [Route("emailChange")]
        public async Task<IActionResult> EmailChange([FromBody] ChangeEmailCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await _mediatR.Send(new LogoutCommand());
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("logoutAll")]
        public async Task<IActionResult> LogoutAll()
        {
            var response = await _mediatR.Send(new LogoutAllCommand());
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("sessions")]
        public async Task<IActionResult> GetSessions()
        {
            var response = await _mediatR.Send(new GetAuthSessionsQuery());
            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        [Route("sessions/{sessionId:guid}")]
        public async Task<IActionResult> RevokeSession(Guid sessionId)
        {
            var response = await _mediatR.Send(new RevokeSessionCommand { SessionId = sessionId });
            return Ok(response);
        }
    }
}
