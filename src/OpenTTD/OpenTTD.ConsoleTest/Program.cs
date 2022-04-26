using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTTD.ConsoleTest;
using OpenTTD.Networking;
using Serilog;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services
        .AddLogging()
        .AddHostedService<AkkaHostedService>()
        .AddAdminPortNetworking())
    .ConfigureLogging((_, builder) => builder
        .ClearProviders()
        .AddSerilog(Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger()))
    .Build()
    .RunAsync();