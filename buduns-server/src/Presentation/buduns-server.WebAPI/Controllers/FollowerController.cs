using buduns_server.Application.Common.Consts;
using buduns_server.Application.Common.CustomAttrributes;
using buduns_server.Application.Features.Followers.Commands.Create;
using buduns_server.Application.Features.Followers.Commands.Delete;
using buduns_server.Application.Features.Followers.Queries.GetAllByUserId;
using buduns_server.Application.Features.Followers.Queries.GetStatus;
using buduns_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace buduns_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowerController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public FollowerController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Followers, ActionType = ActionType.Writing, Definition = "Follow User")]
        [HttpPost("{userId:int}")]
        public async Task<IActionResult> Create(int userId)
        {
            return Ok(await _mediatR.Send(new CreateFollowersCommand { FollowingId = userId }));
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Followers, ActionType = ActionType.Deleting, Definition = "Unfollow User")]
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> Delete(int userId)
        {
            return Ok(await _mediatR.Send(new DeleteFollowersCommand { FollowingId = userId }));
        }

        [HttpGet("{userId:int}/followers")]
        public async Task<IActionResult> GetFollowersByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            return Ok(await _mediatR.Send(new GetAllFollowersByUserIdQuery { UserId = userId, Page = page, Size = size }));
        }

        [HttpGet("{userId:int}/followings")]
        public async Task<IActionResult> GetFollowingsByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            return Ok(await _mediatR.Send(new GetAllFollowingsByUserIdQuery { UserId = userId, Page = page, Size = size }));
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Followers, ActionType = ActionType.Reading, Definition = "Get Follow Status")]
        [HttpGet("status/{userId:int}")]
        public async Task<IActionResult> GetStatus(int userId)
        {
            return Ok(await _mediatR.Send(new GetFollowerStatusQuery { FollowingId = userId }));
        }
    }
}
