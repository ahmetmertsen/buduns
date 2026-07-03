using AutoMapper;
using buduns_server.Application.Dtos;
using buduns_server.Application.Dtos.User;
using buduns_server.Application.Features.Auth.Register;
using buduns_server.Application.Features.Users.Commands.Update.UpdateEmail;
using buduns_server.Application.Features.Users.Commands.Update.UpdatePassword;
using buduns_server.Application.Features.Users.Commands.Update.UpdateProfile;
using buduns_server.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            //Register
            CreateMap<RegisterUserCommand, User>();
            CreateMap<RegisterUserCommand, RegisterUserRequestDto>();
            CreateMap<RegisterUserRequestDto, User>();
            CreateMap<User, RegisterUserCommandResponse>();

            //Update
            CreateMap<UpdateUserProfileCommand, User>();
            CreateMap<UpdateUserEmailCommand, User>();
            CreateMap<UpdateUserPasswordCommand, User>();

            //Dto
            CreateMap<User, UserDto>()
                .ForMember(destination => destination.FullName, options => options.MapFrom(source => source.FullName))
                .ForMember(destination => destination.FollowerCount, options => options.Ignore())
                .ForMember(destination => destination.FollowingCount, options => options.Ignore());
        }
    }
}
