using buduns_server.Application.Common.Consts;
using buduns_server.Application.Common.CustomAttrributes;
using buduns_server.Application.Features.Comments.Commands.Create;
using buduns_server.Application.Features.Comments.Commands.Delete;
using buduns_server.Application.Features.Comments.Commands.Update;
using buduns_server.Application.Features.Comments.Queries.GetById;
using buduns_server.Application.Features.Comments.Queries.GetByPostId;
using buduns_server.Application.Features.Comments.Queries.GetByUserId;
using buduns_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace buduns_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public CommentController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Comments, ActionType = ActionType.Writing, Definition = "Create Comment")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentsCommand request)
        {
            return Ok(await _mediatR.Send(request));
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Comments, ActionType = ActionType.Updating, Definition = "Update Comment")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentsCommand request)
        {
            request.Id = id;
            return Ok(await _mediatR.Send(request));
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Comments, ActionType = ActionType.Deleting, Definition = "Delete Comment")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediatR.Send(new DeleteCommentsCommand { Id = id }));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _mediatR.Send(new GetCommentByIdQuery(id)));
        }

        [HttpGet("post/{postId:int}")]
        public async Task<IActionResult> GetByPostId(int postId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            return Ok(await _mediatR.Send(new GetCommentsByPostIdQuery { PostId = postId, Page = page, Size = size }));
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            return Ok(await _mediatR.Send(new GetCommentsByUserIdQuery { UserId = userId, Page = page, Size = size }));
        }
    }
}
