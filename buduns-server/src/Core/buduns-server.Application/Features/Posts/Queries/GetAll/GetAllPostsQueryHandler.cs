using buduns_server.Application.Common.Helpers;
using buduns_server.Application.Dtos;
using buduns_server.Application.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace buduns_server.Application.Features.Posts.Queries.GetAll
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, PagedResponse<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllPostsQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResponse<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var viewerUserId = HttpContextUserHelper.GetUserId(_httpContextAccessor.HttpContext);
            var result = await _unitOfWork.PostRepository.GetPagedAsync(request.Page, request.Size, request.TagId, request.UserId, request.Search, request.SortBy, viewerUserId, cancellationToken);
            return new PagedResponse<PostDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
