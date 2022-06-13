using Serilog;

void ConfigureLogging(ILoggingBuilder loggingBuilder)
{
    var logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .CreateLogger();
    
    Log.Logger = logger;

    loggingBuilder
        .ClearProviders()
        .AddSerilog(logger);
}

void ConfigureServices(IServiceCollection services, IConfiguration cfg, IHostEnvironment env)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddOptions();
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
    app.UseAuthorization();
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