using AutoMapper;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.Features.Comments.Commands.Create;
using blogapp_server.Application.Features.Comments.Commands.Update;
using blogapp_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Mapping
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(destination => destination.UserName, options => options.MapFrom(source => source.User != null ? source.User.UserName : null))
                .ForMember(destination => destination.UserImageUrl, options => options.MapFrom(source => source.User != null ? source.User.ImageUrl : null))
                .ForMember(destination => destination.UpdatedAt, options => options.MapFrom(source => source.UpdateAt == default ? (DateTime?)null : source.UpdateAt))
                .ForMember(destination => destination.IsEdited, options => options.MapFrom(source => source.UpdateAt != default));
        }
    }
}
