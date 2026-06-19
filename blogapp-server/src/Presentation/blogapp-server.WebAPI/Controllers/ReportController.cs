using blogapp_server.Application.Features.Report.Commands.CreatePostReport;
using blogapp_server.Application.Features.Report.Commands.CreateCommentReport;
using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Common.CustomAttrributes;
using blogapp_server.Application.Features.Report.Commands.CreateUserReport;
using blogapp_server.Application.Features.Report.Commands.ReviewReport;
using blogapp_server.Application.Features.Report.Queries.GetById;
using blogapp_server.Application.Features.Report.Queries.GetReports;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogapp_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public ReportController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Reports, ActionType = ActionType.Writing, Definition = "Create Post Report")]
        [HttpPost("createPostReport")]
        public async Task<IActionResult> CreatePostReport([FromBody] CreatePostReportCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Reports, ActionType = ActionType.Writing, Definition = "Create Comment Report")]
        [HttpPost("createCommentReport")]
        public async Task<IActionResult> CreateCommentReport([FromBody] CreateCommentReportCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Reports, ActionType = ActionType.Writing, Definition = "Create User Report")]
        [HttpPost("createUserReport")]
        public async Task<IActionResult> CreateUserReport([FromBody] CreateUserReportCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Reports, ActionType = ActionType.Reading, Definition = "Get Reports")]
        [HttpGet]
        public async Task<IActionResult> GetReports([FromQuery] GetReportsQuery request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Reports, ActionType = ActionType.Reading, Definition = "Get Report By Id")]
        [HttpGet]
        [Route("getById/{reportId}")]
        public async Task<IActionResult> GetReportById(int reportId)
        {
            var response = await _mediatR.Send(new GetReportByIdQuery { ReportId = reportId });
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [AuthorizeDefinition( Menu = AuthorizeDefinitionConstants.Reports, ActionType = ActionType.Updating, Definition = "Review Report")]
        [HttpPost("review")]
        public async Task<IActionResult> ReviewReport([FromBody] ReviewReportCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }
    }
}
