using AutoMapper;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.Features.Followers.Commands.Create;
using blogapp_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Mapping
{
    public class FollowerProfile : Profile
    {
        public FollowerProfile()
        {
            CreateMap<CreateFollowersCommand, Follower>();

            CreateMap<Follower, FollowerDto>()
                .ForMember(destination => destination.UserId, options => options.MapFrom(source => source.FollowingId))
                .ForMember(destination => destination.UserName, options => options.Ignore())
                .ForMember(destination => destination.FullName, options => options.Ignore())
                .ForMember(destination => destination.Bio, options => options.Ignore())
                .ForMember(destination => destination.ImageUrl, options => options.Ignore())
                .ForMember(destination => destination.FollowedAt, options => options.MapFrom(source => source.CreatedAt));
        }
    }
}
