using blogapp_server.Application.Dtos;
using MediatR;

namespace blogapp_server.Application.Features.Followers.Queries.GetAllByUserId
{
    public class GetAllFollowingsByUserIdQuery : IRequest<PagedResponse<FollowerDto>>
    {
        public int UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
