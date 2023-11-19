using EventBus;
using EventBus.Abstractions;
using EventBusRedis;
using MediatR;
using OpenTTD.Networking;
using OpenTTD.AdminClient.HostedServices;
using OpenTTD.AdminClient.Services;
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
    
    services.AddSingleton<ICoordinatorService, AkkaHostedSystemService>();
    
    services
        .AddAdminPortNetworking();
        
    services.AddHostedService<AkkaHostedSystemService>();

    services.AddMediatR(typeof(Program).Assembly);
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