using buduns_server.Application.Dtos.Configurations;

namespace buduns_server.Application.Abstractions.Services.Configurations
{
    public interface IApplicationService
    {
        List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
