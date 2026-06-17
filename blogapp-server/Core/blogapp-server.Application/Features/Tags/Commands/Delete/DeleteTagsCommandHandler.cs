using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Tags.Commands.Delete
{
    public class DeleteTagsCommandHandler : IRequestHandler<DeleteTagsCommand, DeleteTagsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTagsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteTagsCommandResponse> Handle(DeleteTagsCommand request, CancellationToken cancellationToken)
        {
            var tag = await _unitOfWork.TagRepository.GetVisibleByIdAsync(request.Id, cancellationToken);
            if (tag == null)
            {
                throw new NotFoundException("Tag bulunamadı.");
            }

            tag.isActive = false;
            tag.isDeleted = true;
            tag.UpdateAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new DeleteTagsCommandResponse(Succeeded: true, Message: "Tag başarıyla silindi.");
        }
    }
}
