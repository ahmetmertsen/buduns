using buduns_server.Application.Abstractions.Services;
using buduns_server.Application.Dtos;
using buduns_server.Application.Dtos.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Users.Queries.GetAll
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResponse<AdminUserDto>>
    {
        private readonly IUserService _userService;

        public GetAllUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<PagedResponse<AdminUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userService.GetPagedUsersAsync(request.Page, request.Size, request.Search, request.Status, request.EmailConfirmed, cancellationToken);
            return new PagedResponse<AdminUserDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
