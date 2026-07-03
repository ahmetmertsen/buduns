using buduns_server.Application.Exceptions;
using buduns_server.Application.UnitOfWork;
using buduns_server.Domain.Entities.Identity;
using buduns_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace buduns_server.Application.Features.Followers.Queries.GetStatus
{
    public class GetFollowerStatusQueryHandler : IRequestHandler<GetFollowerStatusQuery, GetFollowerStatusQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public GetFollowerStatusQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<GetFollowerStatusQueryResponse> Handle(GetFollowerStatusQuery request, CancellationToken cancellationToken)
        {
            var followingUser = await _userManager.FindByIdAsync(request.FollowingId.ToString());
            if (followingUser == null || followingUser.Status == UserStatus.Banned)
            {
                throw new NotFoundException("Kullanıcı bulunamadı.");
            }

            var follow = await _unitOfWork.FollowerRepository.GetByUsersAsync(request.UserId, request.FollowingId, cancellationToken);
            return new GetFollowerStatusQueryResponse(IsFollowing: follow != null, FollowId: follow?.Id);
        }
    }
}
