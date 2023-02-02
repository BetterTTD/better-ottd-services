using Microsoft.Extensions.DependencyInjection;

namespace OpenTTD.Domain;

public static class DomainModule
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services.AddTransient<IServerDispatcher, ServerDispatcher>();
    }
}