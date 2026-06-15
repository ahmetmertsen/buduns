using blogapp_server.Application.Dtos;
using MediatR;

namespace blogapp_server.Application.Features.Likes.Queries.GetByPostId
{
    public class GetLikesByPostIdQuery : IRequest<PagedResponse<LikeDto>>
    {
        public int PostId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
