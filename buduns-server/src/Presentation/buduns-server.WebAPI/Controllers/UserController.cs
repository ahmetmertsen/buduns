using buduns_server.Application.Common.Consts;
using buduns_server.Application.Common.CustomAttrributes;
using buduns_server.Application.Features.Auth.Register;
using buduns_server.Application.Features.Users.Commands.AssignRoleToUser;
using buduns_server.Application.Features.Users.Commands.Update.UpdateEmail;
using buduns_server.Application.Features.Users.Commands.Update.UpdateMailVerify;
using buduns_server.Application.Features.Users.Commands.Update.UpdatePassword;
using buduns_server.Application.Features.Users.Commands.Update.UpdateProfile;
using buduns_server.Application.Features.Users.Queries.GetAll;
using buduns_server.Application.Features.Users.Queries.GetById;
using buduns_server.Application.Features.Users.Queries.GetByUsername;
using buduns_server.Application.Features.Users.Queries.GetRolesToUser;
using buduns_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace buduns_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public UserController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("updatePassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Updating, Definition = "Update User Mail Verify")]
        [HttpPost]
        [Route("updateMailVerify")]
        public async Task<IActionResult> UpdateUserMailVerify([FromBody] UpdateUserMailVerifyCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Updating, Definition = "Update User Profile")]
        [HttpPost]
        [Route("updateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Updating, Definition = "Update User Email")]
        [HttpPost]
        [Route("updateUserEmail")]
        public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateUserEmailCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "GetAll Users")]
        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var response = await _mediatR.Send(new GetUserByIdQuery() { UserId = userId });
            return Ok(response);
        }

        [HttpGet]
        [Route("getUserByUsername/{userName}")]
        public async Task<IActionResult> GetUserByUsername(string userName)
        {
            var response = await _mediatR.Send(new GetUserByUsernameQuery() { UserName = userName });
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Get Roles To User")]
        [HttpGet]
        [Route("getRolesToUser/{userId}")]
        public async Task<IActionResult> GetRolesToUser(int userId)
        {
            var response = await _mediatR.Send(new GetRolesToUserQuery() { UserId = userId });
            return Ok(response);
        }


        [Authorize(Roles = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Writing, Definition = "Assign Role To User")]
        [HttpPost]
        [Route("assignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }
    }
}
