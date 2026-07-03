using buduns_server.Application.Dtos;
using buduns_server.Application.UnitOfWork;
using MediatR;

namespace buduns_server.Application.Features.Posts.Queries.GetMyPosts
{
    public class GetMyPostsQueryHandler : IRequestHandler<GetMyPostsQuery, PagedResponse<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyPostsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<PostDto>> Handle(GetMyPostsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PostRepository.GetPagedByUserIdAsync(request.UserId, request.Page, request.Size, request.UserId, cancellationToken);
            return new PagedResponse<PostDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
