using blogapp_server.Application.Dtos;
using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Comments.Queries.GetByUserId
{
    public class GetCommentsByUserIdQueryHandler : IRequestHandler<GetCommentsByUserIdQuery, PagedResponse<CommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCommentsByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<CommentDto>> Handle(GetCommentsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.CommentRepository.GetPagedByUserIdAsync(request.UserId, request.Page, request.Size, cancellationToken);
            return new PagedResponse<CommentDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
