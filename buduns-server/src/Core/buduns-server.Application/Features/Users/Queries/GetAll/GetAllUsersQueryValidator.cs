using FluentValidation;

namespace buduns_server.Application.Features.Users.Queries.GetAll
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1).WithMessage("Page 1 veya daha büyük olmalıdır.");
            RuleFor(query => query.Size).InclusiveBetween(1, 100).WithMessage("Size 1 ile 100 arasında olmalıdır.");
            RuleFor(query => query.Search).MaximumLength(100).When(query => !string.IsNullOrWhiteSpace(query.Search)).WithMessage("Arama metni en fazla 100 karakter olabilir.");
            RuleFor(query => query.Status).IsInEnum().When(query => query.Status.HasValue).WithMessage("Kullanıcı durumu geçersiz.");
        }
    }
}
