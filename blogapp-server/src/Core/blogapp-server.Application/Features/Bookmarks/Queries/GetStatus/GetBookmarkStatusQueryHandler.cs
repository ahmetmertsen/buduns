using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Bookmarks.Queries.GetStatus
{
    public class GetBookmarkStatusQueryHandler : IRequestHandler<GetBookmarkStatusQuery, GetBookmarkStatusQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBookmarkStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetBookmarkStatusQueryResponse> Handle(GetBookmarkStatusQuery request, CancellationToken cancellationToken)
        {
            var bookmark = await _unitOfWork.BookmarkRepository.GetByUserAndPostAsync(request.UserId, request.PostId, cancellationToken);

            return new GetBookmarkStatusQueryResponse(IsBookmarked: bookmark != null, BookmarkId: bookmark?.Id);
        }
    }
}
