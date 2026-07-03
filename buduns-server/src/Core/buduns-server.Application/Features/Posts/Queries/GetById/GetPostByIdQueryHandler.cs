using buduns_server.Application.Common.Helpers;
using buduns_server.Application.Dtos;
using buduns_server.Application.Exceptions;
using buduns_server.Application.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace buduns_server.Application.Features.Posts.Queries.GetById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetPostByIdQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var viewerUserId = HttpContextUserHelper.GetUserId(_httpContextAccessor.HttpContext);
            var post = await _unitOfWork.PostRepository.GetDtoByIdAsync(request.Id, viewerUserId, cancellationToken);
            if (post == null)
            {
                throw new NotFoundException("Post bulunamadı!");
            }

            return post;
        }
    }
}
