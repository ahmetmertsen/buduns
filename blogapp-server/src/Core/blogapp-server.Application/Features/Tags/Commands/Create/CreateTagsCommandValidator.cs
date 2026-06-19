using FluentValidation;

namespace blogapp_server.Application.Features.Tags.Commands.Create
{
    public class CreateTagsCommandValidator : AbstractValidator<CreateTagsCommand>
    {
        public CreateTagsCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tag adı boş olamaz.")
                .MaximumLength(100).WithMessage("Tag adı en fazla 100 karakter olabilir.");
        }
    }
}
