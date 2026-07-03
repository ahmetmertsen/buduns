using buduns_server.Application.Common.Helpers;
using buduns_server.Application.Dtos;
using buduns_server.Application.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace buduns_server.Application.Features.Posts.Queries.GetAllByTagId
{
    public class GetAllPostsByTagIdQueryHandler : IRequestHandler<GetAllPostsByTagIdQuery, PagedResponse<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllPostsByTagIdQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResponse<PostDto>> Handle(GetAllPostsByTagIdQuery request, CancellationToken cancellationToken)
        {
            var viewerUserId = HttpContextUserHelper.GetUserId(_httpContextAccessor.HttpContext);
            var result = await _unitOfWork.PostRepository.GetPagedByTagIdAsync(request.TagId, request.Page, request.Size, viewerUserId, cancellationToken);
            return new PagedResponse<PostDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
