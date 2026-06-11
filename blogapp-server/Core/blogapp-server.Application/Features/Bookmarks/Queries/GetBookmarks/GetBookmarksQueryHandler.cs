using blogapp_server.Application.Dtos;
using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Bookmarks.Queries.GetBookmarks
{
    public class GetBookmarksQueryHandler : IRequestHandler<GetBookmarksQuery, PagedResponse<BookmarkDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBookmarksQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<BookmarkDto>> Handle(GetBookmarksQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.BookmarkRepository.GetPagedByUserIdAsync(request.UserId, request.Page, request.Size, cancellationToken);

            return new PagedResponse<BookmarkDto>
            {
                Items = result.Items,
                Page = request.Page,
                Size = request.Size,
                TotalCount = result.TotalCount
            };
        }
    }
}
