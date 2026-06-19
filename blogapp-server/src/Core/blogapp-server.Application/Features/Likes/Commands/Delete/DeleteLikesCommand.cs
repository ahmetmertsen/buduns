using blogapp_server.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace blogapp_server.Application.Features.Likes.Commands.Delete
{
    public class DeleteLikesCommand : IRequest<DeleteLikesCommandResponse>, ICurrentUserRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public int PostId { get; set; }
    }
}
