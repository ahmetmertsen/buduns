using buduns_server.Application.Abstractions.Services;
using buduns_server.Application.Abstractions.Services.Configurations;
using buduns_server.Application.Abstractions.Token;
using buduns_server.Infrastructure.Services.Configurations;
using buduns_server.Infrastructure.Services.Mail;
using buduns_server.Infrastructure.Services.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IApplicationService, ApplicationService>();

            return services;
        }
    }
}
