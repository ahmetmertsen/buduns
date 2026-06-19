using blogapp_server.Application.Common.Interfaces;
using blogapp_server.Application.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Likes.Queries.GetMyLikes
{
    public class GetMyLikedPostsQuery : IRequest<PagedResponse<LikedPostDto>>, ICurrentUserRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
