using buduns_server.Application.Dtos;
using buduns_server.Application.Exceptions;
using buduns_server.Application.UnitOfWork;
using MediatR;

namespace buduns_server.Application.Features.Tags.Queries.GetById
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, TagDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTagByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TagDto> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _unitOfWork.TagRepository.GetDtoByIdAsync(request.Id, cancellationToken);
            if (response == null)
            {
                throw new NotFoundException("Tag bulunamadı.");
            }

            return response;
        }
    }
}
