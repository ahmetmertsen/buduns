using FluentValidation;

namespace blogapp_server.Application.Features.Posts.Commands.Create
{
    public class CreatePostsCommandValidator : AbstractValidator<CreatePostsCommand>
    {
        public CreatePostsCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.")
                .MaximumLength(1000).WithMessage("İçerik en fazla 1000 karakter olabilir.");

            RuleFor(x => x.CoverImgUrl)
                .NotEmpty().WithMessage("Kapak görseli boş olamaz.")
                .MaximumLength(500).WithMessage("Kapak görseli en fazla 500 karakter olabilir.");

            RuleFor(x => x.TagIds)
                .NotNull().WithMessage("Tag listesi boş olamaz.");

            RuleForEach(x => x.TagIds)
                .GreaterThan(0).WithMessage("Tag Id 0'dan büyük olmalıdır.");
        }
    }
}
