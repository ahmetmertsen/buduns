using FluentValidation;

namespace blogapp_server.Application.Features.Comments.Commands.Update
{
    public class UpdateCommentsCommandValidator : AbstractValidator<UpdateCommentsCommand>
    {
        public UpdateCommentsCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz.")
                .MaximumLength(1000).WithMessage("Yorum içeriği en fazla 1000 karakter olabilir.");
        }
    }
}
