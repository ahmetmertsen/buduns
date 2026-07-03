using FluentValidation;

namespace buduns_server.Application.Features.Tags.Commands.Update
{
    public class UpdateTagsCommandValidator : AbstractValidator<UpdateTagsCommand>
    {
        public UpdateTagsCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tag adı boş olamaz.")
                .MaximumLength(100).WithMessage("Tag adı en fazla 100 karakter olabilir.");
        }
    }
}
