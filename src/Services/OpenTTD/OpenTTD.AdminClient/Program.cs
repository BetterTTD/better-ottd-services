using Domain;
using OpenTTD.Networking;
using OpenTTD.AdminClient.HostedServices;
using OpenTTD.DataAccess;
using OpenTTD.DataAccess.Extensions;
using OpenTTD.DataAccess.Seeder;
using OpenTTD.Domain.Abstractions;
using OpenTTD.Services;
using Serilog;

void ConfigureLogging(ILoggingBuilder builder)
{
    builder.ClearProviders();
    builder.AddSerilog(Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger());
}

void ConfigureServices(IServiceCollection services, IConfiguration cfg, IHostEnvironment env)
{
    services.AddLogging();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddOptions();
    services.AddControllers();

    services
        .AddAdminPortNetworking()
        .AddServices()
        .AddDomain()
        .AddOttdDataAccessModule(cfg.GetConnectionString(OttdDbConnectionString.Key));
        
    services.AddHostedService<AkkaHostedSystemService>();
    services.AddSingleton<IServersSystemService, AkkaHostedSystemService>();
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
ConfigureLogging(builder.Logging);
ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();
ConfigureApplication(app, builder.Environment);
ConfigureRoutes(app);

await app.MigrateDatabaseAsync<OttdContext, OttdDbSeeder>();
await app.RunAsync();