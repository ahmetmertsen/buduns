using blogapp_server.Application.Features.Posts.Commands.Create;
using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Common.CustomAttrributes;
using blogapp_server.Application.Features.Posts.Commands.Delete;
using blogapp_server.Application.Features.Posts.Commands.Update;
using blogapp_server.Application.Features.Posts.Queries.GetAll;
using blogapp_server.Application.Features.Posts.Queries.GetDailyTopPosts;
using blogapp_server.Application.Features.Posts.Queries.GetAllByTagId;
using blogapp_server.Application.Features.Posts.Queries.GetById;
using blogapp_server.Application.Features.Posts.Queries.GetFollowingPosts;
using blogapp_server.Application.Features.Posts.Queries.GetMyPosts;
using blogapp_server.Application.Features.Posts.Queries.GetPostsByUserId;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogapp_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public PostController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Posts, ActionType = ActionType.Writing, Definition = "Create Post")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreatePostsCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Posts, ActionType = ActionType.Updating, Definition = "Update Post")]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdatePostsCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Posts, ActionType = ActionType.Deleting, Definition = "Delete Post")]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] DeletePostsCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPostsQuery request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediatR.Send(new GetPostByIdQuery(id));
            return Ok(response);
        }

        [HttpGet]
        [Route("tag/{tagId:int}")]
        public async Task<IActionResult> GetByTagId(int tagId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var response = await _mediatR.Send(new GetAllPostsByTagIdQuery { TagId = tagId, Page = page, Size = size });
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Posts, ActionType = ActionType.Reading, Definition = "Get My Posts")]
        [HttpGet]
        [Route("me")]
        public async Task<IActionResult> GetMyPosts([FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var response = await _mediatR.Send(new GetMyPostsQuery { Page = page, Size = size });
            return Ok(response);
        }

        [HttpGet]
        [Route("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var response = await _mediatR.Send(new GetPostsByUserIdQuery { UserId = userId, Page = page, Size = size });
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Posts, ActionType = ActionType.Reading, Definition = "Get Following Posts")]
        [HttpGet]
        [Route("following")]
        public async Task<IActionResult> GetFollowingPosts([FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var response = await _mediatR.Send(new GetFollowingPostsQuery { Page = page, Size = size });
            return Ok(response);
        }

        [HttpGet]
        [Route("daily-top50")]
        public async Task<IActionResult> GetDailyTop50()
        {
            var response = await _mediatR.Send(new GetDailyTopPostsQuery());
            return Ok(response);
        }
    }
}
