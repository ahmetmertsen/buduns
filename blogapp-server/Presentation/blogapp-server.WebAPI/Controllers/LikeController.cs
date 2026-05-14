using blogapp_server.Application.Features.Likes.Commands.Create;
using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Common.CustomAttrributes;
using blogapp_server.Application.Features.Likes.Commands.Delete;
using blogapp_server.Application.Features.Likes.Queries.GetById;
using blogapp_server.Application.Features.Likes.Queries.GetByPostId;
using blogapp_server.Application.Features.Likes.Queries.GetByUserId;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogapp_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public LikeController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Writing, Definition = "Create Like")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateLikesCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Deleting, Definition = "Delete Like")]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteLikesCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }


        [Authorize(Roles = "User")]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Reading, Definition = "Get Like By Id")]
        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediatR.Send(new GetLikeByIdRequest(id));
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Reading, Definition = "Get Likes By User Id")]
        [HttpGet]
        [Route("getByUserId")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var response = await _mediatR.Send(new GetLikesByUserIdRequest { UserId = userId});
            return Ok(response);
        }

        [Authorize(Roles = "User")]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Likes, ActionType = ActionType.Reading, Definition = "Get Likes By Post Id")]
        [HttpGet]
        [Route("getByPostId")]
        public async Task<IActionResult> GetByPostId(int postId)
        {
            var response = await _mediatR.Send(new GetLikesByPostIdRequest { PostId = postId });
            return Ok(response);
        }

    }
}
