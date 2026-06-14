using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Entities.Identity;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace blogapp_server.Application.Features.Followers.Commands.Create
{
    public class CreateFollowersCommandHandler : IRequestHandler<CreateFollowersCommand, CreateFollowersCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CreateFollowersCommandHandler> _logger;

        public CreateFollowersCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, ILogger<CreateFollowersCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<CreateFollowersCommandResponse> Handle(CreateFollowersCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == request.FollowingId)
            {
                throw new BadRequestException("Kullanıcı kendisini takip edemez!");
            }

            var followingUser = await _userManager.FindByIdAsync(request.FollowingId.ToString());
            if (followingUser == null)
            {
                throw new NotFoundException("Takip edilecek kullanıcı bulunamadı.");
            }

            if (followingUser.Status == UserStatus.Banned)
            {
                throw new BadRequestException("Bu kullanıcı takip edilemez.");
            }

            var follow = new Follower
            {
                FollowerId = request.UserId,
                FollowingId = request.FollowingId,
                CreatedAt = DateTime.UtcNow,
                isActive = true,
                isDeleted = false
            };

            var notification = new Notification
            {
                Type = NotificationType.NEW_FOLLOWER,
                Message = "Sizi takip etmeye başladı.",
                UserId = request.FollowingId,
                ActorUserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                isActive = true,
                isDeleted = false
            };

            var result = await _unitOfWork.FollowerRepository.CreateIfNotExistsAsync(follow, notification, cancellationToken);
            if (result.Created)
            {
                _logger.LogInformation("User followed. FollowerUserId: {FollowerUserId}, FollowingUserId: {FollowingUserId}, FollowId: {FollowId}", request.UserId, request.FollowingId, result.Follower.Id);
            }

            var message = result.Created ? "Kullanıcı takip edildi." : "Bu kullanıcı zaten takip ediliyor.";
            return new CreateFollowersCommandResponse(Succeeded: true, Message: message, FollowId: result.Follower.Id, AlreadyFollowing: !result.Created);
        }
    }
}
