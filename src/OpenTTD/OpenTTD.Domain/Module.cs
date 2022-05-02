using Microsoft.Extensions.DependencyInjection;

namespace OpenTTD.Domain;

public static class Module
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services.AddTransient<IServerDispatcher, ServerDispatcher>();
    }
}