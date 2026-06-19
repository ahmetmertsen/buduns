using FluentValidation;

namespace blogapp_server.Application.Features.Comments.Commands.Create
{
    public class CreateCommentsCommandValidator : AbstractValidator<CreateCommentsCommand>
    {
        public CreateCommentsCommandValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Post Id 0'dan büyük olmalıdır.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz.")
                .MaximumLength(1000).WithMessage("Yorum içeriği en fazla 1000 karakter olabilir.");
        }
    }
}
