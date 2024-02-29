using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTTD.ConsoleTest;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services.AddHostedService<App>());

host.RunConsoleAsync();