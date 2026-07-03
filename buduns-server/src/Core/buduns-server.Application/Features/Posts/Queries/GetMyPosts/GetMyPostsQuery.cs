using buduns_server.Application.Common.Interfaces;
using buduns_server.Application.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace buduns_server.Application.Features.Posts.Queries.GetMyPosts
{
    public class GetMyPostsQuery : IRequest<PagedResponse<PostDto>>, ICurrentUserRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
