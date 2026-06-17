using AutoMapper;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Posts.Queries.GetAllByTagId
{
    public class GetAllPostsByTagIdQueryHandler : IRequestHandler<GetAllPostsByTagIdQuery, PagedResponse<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPostsByTagIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PostDto>> Handle(GetAllPostsByTagIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PostRepository.GetPagedByTagIdAsync(request.TagId, request.Page, request.Size, cancellationToken);
            return new PagedResponse<PostDto> { Items = _mapper.Map<List<PostDto>>(result.Items), Page = request.Page, Size = request.Size, TotalCount = result.TotalCount };
        }
    }
}
