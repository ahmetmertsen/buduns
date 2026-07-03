using FluentValidation;

namespace buduns_server.Application.Features.Posts.Commands.Update
{
    public class UpdatePostsCommandValidator : AbstractValidator<UpdatePostsCommand>
    {
        public UpdatePostsCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik boş olamaz.").MaximumLength(1000).WithMessage("İçerik en fazla 1000 karakter olabilir.");
            RuleFor(x => x.TagIds).Must(tagIds => tagIds == null || tagIds.Distinct().Count() <= 3).WithMessage("En fazla 3 tag seçilebilir.");
            RuleForEach(x => x.TagIds).GreaterThan(0).WithMessage("Tag Id 0'dan büyük olmalıdır.");
        }
    }
}
