using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadNotificationCountQueryHandler : IRequestHandler<GetUnreadNotificationCountQuery, GetUnreadNotificationCountQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUnreadNotificationCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetUnreadNotificationCountQueryResponse> Handle(GetUnreadNotificationCountQuery request, CancellationToken cancellationToken)
        {
            var unreadCount = await _unitOfWork.NotificationRepository.GetUnreadCountAsync(request.UserId, cancellationToken);
            return new GetUnreadNotificationCountQueryResponse(unreadCount);
        }
    }
}
