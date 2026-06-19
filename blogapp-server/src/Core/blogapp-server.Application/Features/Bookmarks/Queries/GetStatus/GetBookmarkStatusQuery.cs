using blogapp_server.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Bookmarks.Queries.GetStatus
{
    public class GetBookmarkStatusQuery : IRequest<GetBookmarkStatusQueryResponse>, ICurrentUserRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public int PostId { get; set; }
    }
}
