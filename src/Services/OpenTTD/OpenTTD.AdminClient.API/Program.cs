using MassTransit;
using OpenTTD.AdminClient.API.HostedServices;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Networking;
using Serilog;

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

    services.AddSingleton<ICoordinatorService, AkkaHostedSystemService>();

    services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((ctx, configurator) =>
        {
            configurator.Host("localhost", "/", h =>
            {
                h.Username("sa");
                h.Password("p@ssw0rd");
            });
        });
    });
    
    services.AddAdminPortNetworking();
        
    services.AddHostedService<AkkaHostedSystemService>();

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