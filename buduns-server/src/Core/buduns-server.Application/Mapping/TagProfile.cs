using AutoMapper;
using buduns_server.Application.Dtos;
using buduns_server.Application.Features.Tags.Commands.Create;
using buduns_server.Application.Features.Tags.Commands.Update;
using buduns_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile() 
        {
            CreateMap<CreateTagsCommand, Tag>()
                .ForMember(dest => dest.NormalizedName, opt => opt.Ignore());
            CreateMap<UpdateTagsCommand, Tag>()
                .ForMember(dest => dest.NormalizedName, opt => opt.Ignore());

            CreateMap<Tag, TagDto>();
        }
    }
}
