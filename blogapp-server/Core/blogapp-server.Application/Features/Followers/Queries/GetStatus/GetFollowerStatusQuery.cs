using blogapp_server.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Followers.Queries.GetStatus
{
    public class GetFollowerStatusQuery : IRequest<GetFollowerStatusQueryResponse>, ICurrentUserRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public int FollowingId { get; set; }
    }
}
