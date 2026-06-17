using blogapp_server.Application.Common.Helpers;
using blogapp_server.Application.Exceptions;
using blogapp_server.Application.UnitOfWork;
using blogapp_server.Domain.Entities;
using MediatR;

namespace blogapp_server.Application.Features.Tags.Commands.Create
{
    public class CreateTagsCommandHandler : IRequestHandler<CreateTagsCommand, CreateTagsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTagsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTagsCommandResponse> Handle(CreateTagsCommand request, CancellationToken cancellationToken)
        {
            var name = TagNameNormalizer.NormalizeDisplayName(request.Name);
            var normalizedName = TagNameNormalizer.NormalizeKey(request.Name);
            var exists = await _unitOfWork.TagRepository.ExistsByNormalizedNameAsync(normalizedName, cancellationToken: cancellationToken);
            if (exists)
            {
                throw new BadRequestException("Bu tag zaten mevcut.");
            }

            var now = DateTime.UtcNow;
            var tag = new Tag { Name = name, NormalizedName = normalizedName, CreatedAt = now, UpdateAt = now, isActive = true, isDeleted = false };

            await _unitOfWork.TagRepository.AddAsync(tag);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new CreateTagsCommandResponse(Succeeded: true, Message: "Tag başarıyla eklendi.");
        }
    }
}
