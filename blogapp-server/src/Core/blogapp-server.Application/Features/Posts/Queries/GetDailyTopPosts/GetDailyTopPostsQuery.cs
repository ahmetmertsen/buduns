using blogapp_server.Application.Dtos;
using MediatR;

namespace blogapp_server.Application.Features.Posts.Queries.GetDailyTopPosts
{
    public record GetDailyTopPostsQuery : IRequest<List<TopPostDto>>
    {
    }
}
