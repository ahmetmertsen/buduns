using blogapp_server.Application.Abstractions.Services;
using blogapp_server.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Users.Queries.GetById
{
    public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdRequest, UserDto>
    {
        private readonly IUserService _userService;

        public GetUserByIdRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> Handle(GetUserByIdRequest request ,CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserById(request.UserId);
            return user;
        }
    }
}
