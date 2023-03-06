using Confluent.Kafka;
using MediatR;
using OpenTTD.Networking;
using OpenTTD.AdminClient.HostedServices;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain;
using Serilog;

void ConfigureLogging(IServiceProvider sp, LoggerConfiguration loggerCfg, IConfiguration cfg)
{
    loggerCfg
        .ReadFrom.Configuration(cfg)
        .ReadFrom.Services(sp);
}

void ConfigureServices(IServiceCollection services, IConfiguration cfg, IHostEnvironment env)
{
    services.AddSingleton(p =>
    {
        void LogHandler(IProducer<string, string> _, LogMessage msg)
        {
            if (msg.Level < SyslogLevel.Notice)
            {
                Console.WriteLine($"Error occurred: {msg.Level}: {msg.Message}");
            }
        }

        var producer = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            MessageTimeoutMs = 500,
            RetryBackoffMs = 100,
            MessageSendMaxRetries = 5,
            DeliveryReportFields = "key, value, timestamp",
            EnableDeliveryReports = true,
            EnableIdempotence = false,
            Acks = Acks.Leader
        };

        return new ProducerBuilder<string, string>(producer)
            .SetLogHandler(LogHandler)
            .Build();
    });

    services.AddMediatR(typeof(Program), typeof(DomainModule));
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddOptions();
    services.AddControllers();

    services.AddSingleton<ICoordinatorService, AkkaHostedSystemService>();
    
    services
        .AddAdminPortNetworking()
        .AddDomain();
        
    services.AddHostedService<AkkaHostedSystemService>();
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