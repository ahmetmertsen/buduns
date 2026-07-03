using buduns_server.Domain.Entities.Identity;

namespace buduns_server.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        public Dtos.Token CreateAccessToken(User user, IList<string> roles, Guid sessionId, string refreshToken);
        public string CreateRefreshToken();
    }
}
