using Microsoft.Extensions.Hosting;

namespace OpenTTD.ConsoleTest;

public sealed class App : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.Read();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}