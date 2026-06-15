using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Enums;
using MediatR;

namespace blogapp_server.Application.Features.Likes.Commands.Create
{
    public class CreateLikesCommandHandler : IRequestHandler<CreateLikesCommand, CreateLikesCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLikesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateLikesCommandResponse> Handle(CreateLikesCommand request, CancellationToken cancellationToken)
        {
            var postOwnerId = await _unitOfWork.PostRepository.GetVisibleOwnerIdAsync(request.PostId, cancellationToken);
            if (!postOwnerId.HasValue)
            {
                throw new NotFoundException("Beğenilecek paylaşım bulunamadı.");
            }

            var now = DateTime.UtcNow;
            var like = new Like { UserId = request.UserId, PostId = request.PostId, CreatedAt = now, isActive = true, isDeleted = false };
            Notification? notification = null;
            if (postOwnerId.Value != request.UserId)
            {
                notification = new Notification { Type = NotificationType.POST_LIKED, Message = "Paylaşımınız yeni bir beğeni aldı.", UserId = postOwnerId.Value, ActorUserId = request.UserId, PostId = request.PostId, CreatedAt = now, isActive = true, isDeleted = false };
            }

            var result = await _unitOfWork.LikeRepository.CreateIfNotExistsAsync(like, notification, cancellationToken);
            var message = result.Created ? "Paylaşım beğenildi." : "Paylaşım zaten beğenilmiş.";
            return new CreateLikesCommandResponse(Succeeded: true, Message: message, LikeId: result.Like.Id, AlreadyLiked: !result.Created);
        }
    }
}
