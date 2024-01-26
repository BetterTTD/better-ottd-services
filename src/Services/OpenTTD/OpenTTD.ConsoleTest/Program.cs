using EventBus;
using EventBus.Abstractions;
using EventBusRedis;
using IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTTD.ConsoleTest;
using StackExchange.Redis;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services
        .AddScoped<IEventBus, RedisEventBus>()
        .AddScoped<RedisConnection>()
        .AddScoped<IEventBusSubscriptionManager, EventBusSubscriptionManager>()
        .AddEventHandler<ServerMessageReceivedIntegrationEvent, ServerMessageReceivedEventHandler>()
        .AddSingleton<IConnectionMultiplexer>(sp =>
        {
            const string connectionString = "localhost:6379";
            return ConnectionMultiplexer.Connect(connectionString);
        })
        .AddHostedService<App>());

host.RunConsoleAsync();