using buduns_server.Application.Dtos;
using buduns_server.Application.UnitOfWork;
using MediatR;

namespace buduns_server.Application.Features.Tags.Queries.GetAll
{
    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, PagedResponse<TagDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTagsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.TagRepository.GetPagedAsync(request.Page, request.Size, request.Search, cancellationToken);
            return new PagedResponse<TagDto> { Items = result.Items, Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
