using FluentValidation;

namespace blogapp_server.Application.Features.Posts.Queries.GetAll
{
    public class GetAllPostsQueryValidator : AbstractValidator<GetAllPostsQuery>
    {
        private static readonly string[] SortOptions = { "recent", "oldest", "popular" };

        public GetAllPostsQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1).WithMessage("Page 1 veya daha büyük olmalıdır.");
            RuleFor(query => query.Size).InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
            RuleFor(query => query.TagId).GreaterThan(0).When(query => query.TagId.HasValue).WithMessage("Tag Id 0'dan büyük olmalıdır.");
            RuleFor(query => query.UserId).GreaterThan(0).When(query => query.UserId.HasValue).WithMessage("User Id 0'dan büyük olmalıdır.");
            RuleFor(query => query.Search).MaximumLength(100).When(query => !string.IsNullOrWhiteSpace(query.Search)).WithMessage("Arama metni en fazla 100 karakter olabilir.");
            RuleFor(query => query.SortBy).Must(sortBy => string.IsNullOrWhiteSpace(sortBy) || SortOptions.Contains(sortBy.Trim().ToLowerInvariant())).WithMessage("SortBy recent, oldest veya popular olabilir.");
        }
    }
}
