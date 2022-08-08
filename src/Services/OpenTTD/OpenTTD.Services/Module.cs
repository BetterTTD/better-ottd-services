using Microsoft.Extensions.DependencyInjection;
using OpenTTD.Services.Abstractions;

namespace OpenTTD.Services;

public static class Module
{
    public static IServiceCollection AddServices(this IServiceCollection services) => services
        .AddScoped<IServerService, ServerService>();
}