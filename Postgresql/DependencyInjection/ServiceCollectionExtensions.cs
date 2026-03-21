using Microsoft.Extensions.DependencyInjection;

namespace PostgreSql.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        return services;
    }
}