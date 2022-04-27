using System.Net;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Util;
using Microsoft.Extensions.Hosting;
using OpenTTD.Actors.Coordinator;
using OpenTTD.Domain;

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
            .WithConfig("akka { loglevel=DEBUG, loggers=[\"Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog\"]}")
            .And(DependencyResolverSetup.Create(_serviceProvider));

        _actorSystem = ActorSystem.Create("ottd", actorSystemSetup);

        var coordinatorProps = DependencyResolver.For(_actorSystem).Props<CoordinatorActor>();
        _coordinator = _actorSystem.ActorOf(coordinatorProps, "coordinator");

        _actorSystem.WhenTerminated.ContinueWith(_ => { _appLifetime.StopApplication(); }, cancellationToken);

        Test(cancellationToken);
        
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

    private async void Test(CancellationToken cancellationToken)
    {
        try
        {
            var msg = new ServerAdd(new ServerCredentials
            {
                NetworkAddress = new NetworkAddress(IPAddress.Parse("127.0.0.1"), 3977),
                Name = "TG Admin",
                Version = "1.0",
                Password = "12345"
            });
            
            var guid = await _coordinator.Ask<Result<ServerAdded>>(msg, cancellationToken: cancellationToken);
            var guid2 = await _coordinator.Ask<Result<ServerAdded>>(msg, cancellationToken: cancellationToken);
            
            _coordinator.Tell(new ServerConnect(guid.Value.ServerIdentifier));
            
        }
        catch (Exception enx)
        {
            Console.WriteLine(enx);
            throw;
        }
    }
}