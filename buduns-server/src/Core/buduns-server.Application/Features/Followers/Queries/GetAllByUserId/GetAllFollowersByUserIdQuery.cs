using buduns_server.Application.Dtos;
using MediatR;

namespace buduns_server.Application.Features.Followers.Queries.GetAllByUserId
{
    public class GetAllFollowersByUserIdQuery : IRequest<PagedResponse<FollowerDto>>
    {
        public int UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
