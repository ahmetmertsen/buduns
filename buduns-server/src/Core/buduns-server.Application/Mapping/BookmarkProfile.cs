using AutoMapper;
using buduns_server.Application.Dtos;
using buduns_server.Application.Features.Bookmarks.Commands.Create;
using buduns_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Mapping
{
    public class BookmarkProfile : Profile
    {
        public BookmarkProfile()
        {
            CreateMap<CreateBookmarksCommand, Bookmark>();
        }
    }
}
