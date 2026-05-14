using blogapp_server.Application.Common.Consts;
using blogapp_server.Application.Common.CustomAttrributes;
using blogapp_server.Application.Features.Roles.Commands.Create;
using blogapp_server.Application.Features.Roles.Commands.Delete;
using blogapp_server.Application.Features.Roles.Commands.Update;
using blogapp_server.Application.Features.Roles.Queries.GetAll;
using blogapp_server.Application.Features.Roles.Queries.GetById;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogapp_server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public RoleController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Roles, ActionType = ActionType.Reading, Definition = "GetAll Roles")]
        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediatR.Send(new GetAllRolesRequest());
            return Ok(response);
        }

        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Roles, ActionType = ActionType.Reading, Definition = "Get Role By Id")]
        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediatR.Send(new GetRoleByIdRequest { Id = id });
            return Ok(response);
        }

        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Roles, ActionType = ActionType.Writing, Definition = "Create Role")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Roles, ActionType = ActionType.Updating, Definition = "Update Role")]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Roles, ActionType = ActionType.Deleting, Definition = "Delete Role")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediatR.Send(new DeleteRoleCommand { Id = id });
            return Ok(response);
        }
    }
}
