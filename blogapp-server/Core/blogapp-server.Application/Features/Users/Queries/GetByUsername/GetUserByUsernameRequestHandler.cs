using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.Features.Users.Queries.GetById;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Users.Queries.GetByUsername
{
    public class GetUserByUsernameRequestHandler : IRequestHandler<GetUserByUsernameRequest, UserDto>
    {
        private readonly IUserService _userService;

        public GetUserByUsernameRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> Handle(GetUserByUsernameRequest request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByUserName(request.UserName);
            return user;
        }
    }
}
