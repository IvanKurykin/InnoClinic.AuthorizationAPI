using BLL.Interfaces;
using BLL.Services;
using DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddBLLServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDALServices(configuration);
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
