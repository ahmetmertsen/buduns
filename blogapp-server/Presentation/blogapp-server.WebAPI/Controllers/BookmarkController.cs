using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Common.CustomAttrributes;
using blogapp_server.Application.Features.Bookmarks.Commands.Create;
using blogapp_server.Application.Features.Bookmarks.Commands.Delete;
using blogapp_server.Application.Features.Bookmarks.Queries.GetBookmarks;
using blogapp_server.Application.Features.Bookmarks.Queries.GetStatus;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogapp_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public BookmarkController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Bookmarks, ActionType = ActionType.Writing, Definition = "Create Bookmark")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookmarksCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Bookmarks, ActionType = ActionType.Deleting, Definition = "Delete Bookmark")]
        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> Delete(int postId)
        {
            var response = await _mediatR.Send(new DeleteBookmarksCommand { PostId = postId });
            return Ok(response);
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Bookmarks, ActionType = ActionType.Reading, Definition = "Get Bookmarks")]
        [HttpGet]
        public async Task<IActionResult> GetBookmarks([FromQuery] GetBookmarksQuery request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Bookmarks, ActionType = ActionType.Reading, Definition = "Get Bookmark Status")]
        [HttpGet("status/{postId:int}")]
        public async Task<IActionResult> GetStatus(int postId)
        {
            var response = await _mediatR.Send(new GetBookmarkStatusQuery { PostId = postId });
            return Ok(response);
        }
    }
}
