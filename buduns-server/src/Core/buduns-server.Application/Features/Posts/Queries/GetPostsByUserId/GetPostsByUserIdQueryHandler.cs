using buduns_server.Application.Common.Helpers;
using buduns_server.Application.Dtos;
using buduns_server.Application.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace buduns_server.Application.Features.Posts.Queries.GetPostsByUserId
{
    public class GetPostsByUserIdQueryHandler : IRequestHandler<GetPostsByUserIdQuery, PagedResponse<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetPostsByUserIdQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResponse<PostDto>> Handle(GetPostsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var viewerUserId = HttpContextUserHelper.GetUserId(_httpContextAccessor.HttpContext);
            var result = await _unitOfWork.PostRepository.GetPagedByUserIdAsync(request.UserId, request.Page, request.Size, viewerUserId, cancellationToken);
            return new PagedResponse<PostDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
