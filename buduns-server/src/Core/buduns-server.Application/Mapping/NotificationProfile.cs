using AutoMapper;
using buduns_server.Application.Dtos;
using buduns_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(destination => destination.ActorUserName, options => options.MapFrom(source => source.ActorUser != null ? source.ActorUser.UserName : null));
        }
    }
}
