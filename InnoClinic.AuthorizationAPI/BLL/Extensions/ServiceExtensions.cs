using BLL.Interfaces;
using BLL.Mapper;
using BLL.Services;
using DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddBusinessLoginLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessLayerServices(configuration);
        services.AddScoped<IAuthService, AuthService>();
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
