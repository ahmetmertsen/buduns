using blogapp_server.Application.Features.Comments.Commands.Create;
using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Common.CustomAttrributes;
using blogapp_server.Application.Features.Comments.Commands.Delete;
using blogapp_server.Application.Features.Comments.Commands.Update;
using blogapp_server.Application.Features.Comments.Queries.GetById;
using blogapp_server.Application.Features.Comments.Queries.GetByPostId;
using blogapp_server.Application.Features.Comments.Queries.GetByUserId;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogapp_server.WebAPI.Controllers
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
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateCommentsCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Comments, ActionType = ActionType.Updating, Definition = "Update Comment")]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentsCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Comments, ActionType = ActionType.Deleting, Definition = "Delete Comment")]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCommentsCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediatR.Send(new GetCommentByIdRequest(id));
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Comments, ActionType = ActionType.Reading, Definition = "Get Comments By User Id")]
        [HttpGet]
        [Route("getByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var response = await _mediatR.Send(new GetCommentsByUserIdRequest { UserId = userId});
            return Ok(response);
        }

        [HttpGet]
        [Route("getByPostId/{postId}")]
        public async Task<IActionResult> GetByPostId(int postId)
        {
            var response = await _mediatR.Send(new GetCommentsByPostIdRequest { PostId = postId });
            return Ok(response);
        }



    }
}
