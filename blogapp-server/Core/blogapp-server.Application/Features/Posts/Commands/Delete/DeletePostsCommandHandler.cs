using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Posts.Commands.Delete
{
    public class DeletePostsCommandHandler : IRequestHandler<DeletePostsCommand,DeletePostsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePostsCommandHandler> _logger;
        
        public DeletePostsCommandHandler(IUnitOfWork unitOfWork, ILogger<DeletePostsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<DeletePostsCommandResponse> Handle(DeletePostsCommand request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);
            if (post == null)
            {
                throw new NotFoundException("Post bulunamadı!");
            }
            if (post.UserId != request.UserId)
            {
                throw new UnauthorizedAccesException("Bu postu silme yetkiniz yok.");
            }

            await _unitOfWork.PostRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Post deleted. PostId: {PostId}, UserId: {UserId}",
                request.Id,
                request.UserId);

            return new DeletePostsCommandResponse(true, "Post başarıyla silinmiştir.");
        }
    }
}
