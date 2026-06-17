using blogapp_server.Application.Dtos;
using MediatR;

namespace blogapp_server.Application.Features.Posts.Queries.GetPostsByUserId
{
    public class GetPostsByUserIdQuery : IRequest<PagedResponse<PostDto>>
    {
        public int UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
