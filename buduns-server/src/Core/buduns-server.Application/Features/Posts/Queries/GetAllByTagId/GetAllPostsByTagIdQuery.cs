using buduns_server.Application.Dtos;
using MediatR;

namespace buduns_server.Application.Features.Posts.Queries.GetAllByTagId
{
    public class GetAllPostsByTagIdQuery : IRequest<PagedResponse<PostDto>>
    {
        public int TagId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
