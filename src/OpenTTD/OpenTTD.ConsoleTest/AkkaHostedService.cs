using Akka.Actor;
using Akka.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTTD.Actors.Coordinator;

namespace OpenTTD.ConsoleTest;

public class AkkaHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _appLifetime;

    private ActorSystem _actorSystem = null!;
    private IActorRef _coordinator = null!;

    public AkkaHostedService(IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime)
    {
        _serviceProvider = serviceProvider;
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var actorSystemSetup = BootstrapSetup
            .Create()
            .WithConfig("akka { loglevel=INFO,  loggers=[\"Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog\"]}")
            .And(DependencyResolverSetup.Create(_serviceProvider));

        _actorSystem = ActorSystem.Create("ottd", actorSystemSetup);

        var coordinatorProps = DependencyResolver.For(_actorSystem).Props<CoordinatorActor>();
        _coordinator = _actorSystem.ActorOf(coordinatorProps, "coordinator");

        _actorSystem.WhenTerminated.ContinueWith(_ => { _appLifetime.StopApplication(); }, cancellationToken);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_actorSystem is null)
            throw new ArgumentNullException(nameof(_actorSystem));

        await CoordinatedShutdown
            .Get(_actorSystem)
            .Run(CoordinatedShutdown.ClrExitReason.Instance);
    }
}