using Domain;
using Networking;
using OpenTTD.AdminClient.HostedServices;
using OpenTTD.DataAccess;
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

    services.AddAdminPortNetworking();
    services.AddDomain();
    services.AddOttdDataAccessModule(cfg.GetConnectionString(OttdDbConnectionString.Key));
        
    services.AddHostedService<AkkaHostedService>();
}

void ConfigureApplication(IApplicationBuilder app, IHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        //app.UseSwagger();
        //app.UseSwaggerUI();
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
    
}

var builder = WebApplication.CreateBuilder(args);
ConfigureLogging(builder.Logging);
ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();
ConfigureApplication(app, builder.Environment);
ConfigureRoutes(app);

await app.RunAsync();