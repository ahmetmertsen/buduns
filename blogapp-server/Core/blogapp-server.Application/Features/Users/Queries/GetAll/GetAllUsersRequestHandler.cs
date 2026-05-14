using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Users.Queries.GetAll
{
    public class GetAllUsersRequestHandler : IRequestHandler<GetAllUsersRequest, List<UserDto>>
    {
        private readonly IUserService _userService;

        public GetAllUsersRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync();
            return users;
        }
    }
}
