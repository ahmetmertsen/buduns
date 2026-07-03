using FluentValidation;

namespace buduns_server.Application.Features.Tags.Commands.Delete
{
    public class DeleteTagsCommandValidator : AbstractValidator<DeleteTagsCommand>
    {
        public DeleteTagsCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
        }
    }
}
