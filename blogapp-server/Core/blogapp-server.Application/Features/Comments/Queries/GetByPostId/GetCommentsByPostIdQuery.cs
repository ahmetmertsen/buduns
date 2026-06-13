using blogapp_server.Application.Dtos;
using MediatR;

namespace blogapp_server.Application.Features.Comments.Queries.GetByPostId
{
    public class GetCommentsByPostIdQuery : IRequest<PagedResponse<CommentDto>>
    {
        public int PostId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
