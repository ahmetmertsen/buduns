using blogapp_server.Application.Dtos;
using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Likes.Queries.GetByPostId
{
    public class GetLikesByPostIdQueryHandler : IRequestHandler<GetLikesByPostIdQuery, PagedResponse<LikeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLikesByPostIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<LikeDto>> Handle(GetLikesByPostIdQuery request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.PostRepository.ExistsVisibleAsync(request.PostId, cancellationToken))
            {
                throw new NotFoundException("Paylaşım bulunamadı.");
            }

            var result = await _unitOfWork.LikeRepository.GetPagedByPostIdAsync(request.PostId, request.Page, request.Size, cancellationToken);
            return new PagedResponse<LikeDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
