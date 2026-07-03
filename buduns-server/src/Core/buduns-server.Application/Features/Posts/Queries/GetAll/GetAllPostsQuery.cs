using buduns_server.Application.Dtos;
using MediatR;

namespace buduns_server.Application.Features.Posts.Queries.GetAll
{
    public class GetAllPostsQuery : IRequest<PagedResponse<PostDto>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
        public int? TagId { get; set; }
        public int? UserId { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; } = "recent";
    }
}
