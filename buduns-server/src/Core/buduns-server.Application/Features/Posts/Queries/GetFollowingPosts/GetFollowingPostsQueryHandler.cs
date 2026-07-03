using buduns_server.Application.Dtos;
using buduns_server.Application.UnitOfWork;
using MediatR;

namespace buduns_server.Application.Features.Posts.Queries.GetFollowingPosts
{
    public class GetFollowingPostsQueryHandler : IRequestHandler<GetFollowingPostsQuery, PagedResponse<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFollowingPostsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<PostDto>> Handle(GetFollowingPostsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PostRepository.GetPagedFollowingAsync(request.UserId, request.Page, request.Size, cancellationToken);
            return new PagedResponse<PostDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
