using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus;

public static class Config
{
    public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(
        this IServiceCollection services
    )
        where TEvent : IntegrationEvent
        where TEventHandler : class, IIntegrationEventHandler<TEvent>
    {
        return services
            .AddTransient<TEventHandler>()
            .AddTransient<IIntegrationEventHandler<TEvent>>(sp => sp.GetRequiredService<TEventHandler>());
    }
}