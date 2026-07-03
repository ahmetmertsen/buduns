using buduns_server.Application.Common.Consts;
using buduns_server.Application.Common.CustomAttrributes;
using buduns_server.Application.Features.Likes.Commands.Create;
using buduns_server.Application.Features.Likes.Commands.Delete;
using buduns_server.Application.Features.Likes.Queries.GetByPostId;
using buduns_server.Application.Features.Likes.Queries.GetMyLikes;
using buduns_server.Application.Features.Likes.Queries.GetStatus;
using buduns_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace buduns_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public LikeController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Writing, Definition = "Create Like")]
        [HttpPost("{postId:int}")]
        public async Task<IActionResult> Create(int postId)
        {
            return Ok(await _mediatR.Send(new CreateLikesCommand { PostId = postId }));
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Deleting, Definition = "Delete Like")]
        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> Delete(int postId)
        {
            return Ok(await _mediatR.Send(new DeleteLikesCommand { PostId = postId }));
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Reading, Definition = "Get Like Status")]
        [HttpGet("status/{postId:int}")]
        public async Task<IActionResult> GetStatus(int postId)
        {
            return Ok(await _mediatR.Send(new GetLikeStatusQuery { PostId = postId }));
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Reading, Definition = "Get Likes By Post Id")]
        [HttpGet("post/{postId:int}")]
        public async Task<IActionResult> GetByPostId(int postId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            return Ok(await _mediatR.Send(new GetLikesByPostIdQuery { PostId = postId, Page = page, Size = size }));
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Reading, Definition = "Get My Liked Posts")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyLikedPosts([FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            return Ok(await _mediatR.Send(new GetMyLikedPostsQuery { Page = page, Size = size }));
        }
    }
}
