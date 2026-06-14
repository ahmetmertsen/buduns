using blogapp_server.Application.Dtos;
using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using blogapp_server.Domain.Entities.Identity;
using blogapp_server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace blogapp_server.Application.Features.Followers.Queries.GetAllByUserId
{
    public class GetAllFollowersByUserIdQueryHandler : IRequestHandler<GetAllFollowersByUserIdQuery, PagedResponse<FollowerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public GetAllFollowersByUserIdQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<PagedResponse<FollowerDto>> Handle(GetAllFollowersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null || user.Status == UserStatus.Banned)
            {
                throw new NotFoundException("Kullanıcı bulunamadı.");
            }

            var result = await _unitOfWork.FollowerRepository.GetPagedFollowersByUserIdAsync(request.UserId, request.Page, request.Size, cancellationToken);
            return new PagedResponse<FollowerDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
