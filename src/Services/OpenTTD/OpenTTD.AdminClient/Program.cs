using System.Reflection;
using Confluent.Kafka;
using MediatR;
using OpenTTD.Networking;
using OpenTTD.AdminClient.HostedServices;
using OpenTTD.Domain;
using OpenTTD.Domain.Events;
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
    services.AddLogging();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddOptions();
    services.AddControllers();

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
ConfigureLogging(builder.Logging);
ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();
ConfigureApplication(app, builder.Environment);
ConfigureRoutes(app);

await app.RunAsync();