using blogapp_server.Application.Dtos;
using MediatR;

namespace blogapp_server.Application.Features.Tags.Queries.GetAll
{
    public class GetAllTagsQuery : IRequest<PagedResponse<TagDto>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 50;
        public string? Search { get; set; }
    }
}
