using blogapp_server.Application.Dtos;
using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Comments.Queries.GetById
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCommentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommentDto> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CommentRepository.GetDtoByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Yorum bulunamadı.");
        }
    }
}
