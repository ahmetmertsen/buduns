using buduns_server.Domain.Enums;

namespace buduns_server.Application.Abstractions.Services
{
    public interface IVerificationChallengeService
    {
        Task<string> CreateCodeAsync(int userId, VerificationPurpose purpose, string? targetEmail, CancellationToken cancellationToken);
        Task ValidateCodeAsync(int userId, VerificationPurpose purpose, string? targetEmail, string code, CancellationToken cancellationToken);
        Task ValidateEmailChangeCodesAsync(int userId, string targetEmail, string oldEmailCode, string newEmailCode, CancellationToken cancellationToken);
    }
}
