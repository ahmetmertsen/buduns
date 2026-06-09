using blogapp_server.Application.Exceptions;
using blogapp_server.Application.Features.Posts.Commands.Delete;
using blogapp_server.Application.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Comments.Commands.Delete
{
    public class DeleteCommentsCommandHandler : IRequestHandler<DeleteCommentsCommand, DeleteCommentsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCommentsCommandHandler> _logger;

        public DeleteCommentsCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCommentsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<DeleteCommentsCommandResponse> Handle(DeleteCommentsCommand request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.Id);
            if (comment == null)
            {
                throw new NotFoundException("Yorum bulunamadı!");
            }
            if (comment.UserId != request.UserId)
            {
                throw new UnauthorizedAccesException("Bu yorumu silme yetkiniz yok.");
            }

            await _unitOfWork.CommentRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Comment deleted. CommentId: {CommentId}, PostId: {PostId}, UserId: {UserId}",
                request.Id,
                comment.PostId,
                request.UserId);

            return new DeleteCommentsCommandResponse(true, "Yorum başarıyla silinmiştir.");
        }
    }
}
