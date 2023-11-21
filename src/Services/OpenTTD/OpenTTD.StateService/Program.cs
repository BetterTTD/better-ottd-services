using EventBus;
using EventBus.Abstractions;
using EventBusRedis;
using IntegrationEvents;
using OpenTTD.StateService.Handlers;
using OpenTTD.StateService.HostedService;
using Serilog;
using StackExchange.Redis;

void ConfigureLogging(IServiceProvider sp, LoggerConfiguration loggerCfg, IConfiguration cfg)
{
    loggerCfg
        .ReadFrom.Configuration(cfg)
        .ReadFrom.Services(sp);
}

void ConfigureServices(IServiceCollection services, IConfiguration cfg, IHostEnvironment env)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddOptions();
    services.AddControllers();

    services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var connectionString = cfg.GetConnectionString("RedisConnectionString");
        if (connectionString is null)
        {
            throw new ArgumentNullException(connectionString);
        }
        return ConnectionMultiplexer.Connect(connectionString);
    });

    services
        .AddSingleton<IEventBus, RedisEventBus>()
        .AddSingleton<RedisConnection>()
        .AddSingleton<IEventBusSubscriptionManager, EventBusSubscriptionManager>();

    services.AddEventHandler<ServerMessageReceivedEvent, ServerMessageReceivedEventHandler>();

    services.AddHostedService<EventBusHostedService>();
    
    services.AddMediatR(c => c.RegisterServicesFromAssemblies(typeof(Program).Assembly));
}

void ConfigureApplication(IApplicationBuilder app, IHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHsts();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseHttpsRedirection();
    app.UseRouting();
}

void ConfigureRoutes(IEndpointRouteBuilder router)
{
    router.MapControllers();
}

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Host.UseSerilog(
    (_, sp, logCfg) => ConfigureLogging(sp, logCfg, builder.Configuration), 
    writeToProviders: true);
ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();
ConfigureApplication(app, builder.Environment);
ConfigureRoutes(app);

await app.RunAsync();