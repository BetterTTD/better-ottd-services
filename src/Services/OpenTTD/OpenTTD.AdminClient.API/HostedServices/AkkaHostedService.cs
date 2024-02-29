using System.Net;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Util;
using OpenTTD.AdminClient.Actors;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.HostedServices;

public sealed class AkkaHostedSystemService(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime appLifetime)
    : IHostedService, ICoordinatorService
{
    private ActorSystem _actorSystem = null!;
    private IActorRef _coordinator = null!;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var actorSystemSetup = BootstrapSetup
            .Create()
            .WithConfig("akka { loglevel=DEBUG, loggers=[\"Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog\"]}")
            .And(DependencyResolverSetup.Create(serviceProvider));

        _actorSystem = ActorSystem.Create("ottd", actorSystemSetup);
        
        var coordinatorProps = DependencyResolver.For(_actorSystem).Props<CoordinatorActor>();
        _coordinator = _actorSystem.ActorOf(coordinatorProps, "coordinator");

        await _actorSystem.WhenTerminated.ContinueWith(_ => appLifetime.StopApplication(), cancellationToken);

        await Test(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_actorSystem is null)
            throw new ArgumentNullException(nameof(_actorSystem));

        await CoordinatedShutdown
            .Get(_actorSystem)
            .Run(CoordinatedShutdown.ClrExitReason.Instance);
    }

    public async Task<Result<ServerId>> AskToAddServerAsync(ServerId id, ServerNetwork network, CancellationToken cts)
    {
        var result = await _coordinator.Ask<Result<ServerAdded>>(new ServerAdd(id ,network), cancellationToken: cts);
        
        return result.IsSuccess
            ? Result.Success(result.Value.Id)
            : Result.Failure<ServerId>(result.Exception);
    }

    public async Task TellToConnectServerAsync(ServerId id, CancellationToken cts)
    {
        await Task.Run(() => _coordinator.Tell(new ServerConnect(id)), cts);
    }

    public async Task TellToDisconnectServerAsync(ServerId id, CancellationToken cts)
    {
        await Task.Run(() => _coordinator.Tell(new ServerDisconnect(id)), cts);
    }

    public async Task TellToRemoveServerAsync(ServerId id, CancellationToken cts)
    {
        await Task.Run(() => _coordinator.Tell(new ServerRemove(id)), cts);
    }
    
    private async Task Test(CancellationToken cancellationToken)
    {
        try
        {
            var msg = new ServerAdd(
                new ServerId(Guid.NewGuid()),
                new ServerNetwork
                {
                    NetworkAddress = new NetworkAddress(IPAddress.Parse("127.0.0.1"), new ServerPort(3977)),
                    AdminName = new ServerAdminName("TG Admin"),
                    Version = new ServerVersion("1.0"),
                    Password = new ServerPassword("12345")
                });
            
            var result = await _coordinator.Ask<Result<ServerAdded>>(msg, cancellationToken: cancellationToken);

            _coordinator.Tell(new ServerConnect(result.Value.Id));
        }
        catch (Exception enx)
        {
            Console.WriteLine(enx);
            throw;
        }
    }
}