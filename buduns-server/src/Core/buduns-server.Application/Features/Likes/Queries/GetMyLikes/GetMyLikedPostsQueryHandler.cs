using buduns_server.Application.Dtos;
using buduns_server.Application.UnitOfWork;
using MediatR;

namespace buduns_server.Application.Features.Likes.Queries.GetMyLikes
{
    public class GetMyLikedPostsQueryHandler : IRequestHandler<GetMyLikedPostsQuery, PagedResponse<LikedPostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyLikedPostsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<LikedPostDto>> Handle(GetMyLikedPostsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.LikeRepository.GetPagedByUserIdAsync(request.UserId, request.Page, request.Size, cancellationToken);
            return new PagedResponse<LikedPostDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
