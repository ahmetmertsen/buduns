using buduns_server.Application.Dtos;
using buduns_server.Application.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Notifications.Queries.GetAllByUserId
{
    public class GetAllNotificationsByUserIdQueryHandler : IRequestHandler<GetAllNotificationsByUserIdQuery, PagedResponse<NotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllNotificationsByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<NotificationDto>> Handle(GetAllNotificationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.NotificationRepository.GetPagedByUserIdAsync(request.UserId, request.Page, request.Size, request.OnlyUnread, cancellationToken);
            return new PagedResponse<NotificationDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
