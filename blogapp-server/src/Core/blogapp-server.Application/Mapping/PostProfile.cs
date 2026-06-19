using AutoMapper;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.Features.Posts.Commands.Create;
using blogapp_server.Application.Features.Posts.Commands.Update;
using blogapp_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Mapping
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<CreatePostsCommand, Post>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<UpdatePostsCommand, Post>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName ?? string.Empty))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserImageUrl, opt => opt.MapFrom(src => src.User.ImageUrl))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdateAt))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Where(tag => tag.isActive && !tag.isDeleted)))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count(like => like.isActive && !like.isDeleted && like.User.Status != Domain.Enums.UserStatus.Banned)))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count(comment => comment.Status == Domain.Enums.CommentStatus.Published && comment.isActive && !comment.isDeleted)))
                .ForMember(dest => dest.BookmarkCount, opt => opt.MapFrom(src => src.Bookmarks.Count(bookmark => bookmark.isActive && !bookmark.isDeleted)))
                .ForMember(dest => dest.IsLiked, opt => opt.Ignore())
                .ForMember(dest => dest.IsBookmarked, opt => opt.Ignore())
                .ForMember(dest => dest.IsOwner, opt => opt.Ignore())
                .ForMember(dest => dest.IsFollowingAuthor, opt => opt.Ignore());
        }
    }
}
