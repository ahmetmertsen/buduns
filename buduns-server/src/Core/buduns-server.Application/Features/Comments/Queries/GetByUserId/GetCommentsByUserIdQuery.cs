using buduns_server.Application.Dtos;
using MediatR;

namespace buduns_server.Application.Features.Comments.Queries.GetByUserId
{
    public class GetCommentsByUserIdQuery : IRequest<PagedResponse<CommentDto>>
    {
        public int UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
