using buduns_server.Application.Dtos;
using MediatR;

namespace buduns_server.Application.Features.Posts.Queries.GetDailyTopPosts
{
    public record GetDailyTopPostsQuery : IRequest<List<TopPostDto>>
    {
    }
}
