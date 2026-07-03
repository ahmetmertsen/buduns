using AutoMapper;
using buduns_server.Application.Abstractions.Services;
using buduns_server.Application.Dtos.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            RegisterUserRequestDto userDto = _mapper.Map<RegisterUserRequestDto>(request);
            RegisterUserResponseDto response = await _userService.RegisterAsync(userDto, cancellationToken);
            if (response.Succeeded)
            {
                return new RegisterUserCommandResponse(Succeeded: true, Message: response.Message);
            }
            else
            {
                return new RegisterUserCommandResponse(Succeeded: response.Succeeded, Message: response.Message);
            }
        }
    }
}
