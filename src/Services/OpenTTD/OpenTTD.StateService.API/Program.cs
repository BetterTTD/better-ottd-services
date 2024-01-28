using MassTransit;
using OpenTTD.AdminClient.Networking;
using OpenTTD.StateService.DataAccess;
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

    services.AddMassTransit(busCfg =>
    {
        busCfg.SetKebabCaseEndpointNameFormatter();
        busCfg.AddConsumers(typeof(Program).Assembly);
        busCfg.UsingRabbitMq((ctx, rabbitCfg) =>
        {
            rabbitCfg.ConfigureEndpoints(ctx);
            rabbitCfg.Host("localhost", "/", h =>
            {
                h.Username("sa");
                h.Password("p@ssw0rd");
            });
        });
    });

    services
        .AddScoped<INetworkMessageDeserializer, NetworkMessageDeserializer>()
        .AddServerDb();

    services
        .Configure<ServerDbConnectionString>(x =>
            x.Value = cfg.GetConnectionString(nameof(ServerDbConnectionString)) ??
                      throw new ArgumentNullException(x.Value, "Server DB connection string is null"));
    
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

await app.MigrateDatabaseAsync();
await app.RunAsync();