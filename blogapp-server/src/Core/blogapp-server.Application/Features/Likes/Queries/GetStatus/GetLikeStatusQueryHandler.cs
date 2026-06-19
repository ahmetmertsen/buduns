using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Likes.Queries.GetStatus
{
    public class GetLikeStatusQueryHandler : IRequestHandler<GetLikeStatusQuery, GetLikeStatusQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLikeStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetLikeStatusQueryResponse> Handle(GetLikeStatusQuery request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.PostRepository.ExistsVisibleAsync(request.PostId, cancellationToken))
            {
                throw new NotFoundException("Paylaşım bulunamadı.");
            }

            var like = await _unitOfWork.LikeRepository.GetByUserAndPostAsync(request.UserId, request.PostId, cancellationToken);
            return new GetLikeStatusQueryResponse(IsLiked: like != null, LikeId: like?.Id);
        }
    }
}
