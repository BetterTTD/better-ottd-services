using System.Net;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Util;
using OpenTTD.Actors.Coordinator;
using OpenTTD.AdminClient.Services;
using OpenTTD.AdminClientDomain.Models;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClient.HostedServices;

public sealed class AkkaHostedSystemService : IHostedService, ICoordinatorService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _appLifetime;

    private ActorSystem _actorSystem = null!;
    private IActorRef _coordinator = null!;

    public AkkaHostedSystemService(
        IServiceProvider serviceProvider, 
        IHostApplicationLifetime appLifetime)
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

        _actorSystem.WhenTerminated.ContinueWith(_ => _appLifetime.StopApplication(), cancellationToken);

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

    public async Task<Result<ServerId>> AskToAddServerAsync(ServerCredentials credentials, CancellationToken cts)
    {
        var result = await _coordinator.Ask<Result<ServerAdded>>(new ServerAdd(credentials), cancellationToken: cts);
        
        return result.IsSuccess
            ? Result.Success(result.Value.Id)
            : Result.Failure<ServerId>(result.Exception);
    }

    public Task TellServerToConnectAsync(ServerId id, CancellationToken cts)
    {
        _coordinator.Tell(new ServerConnect(id));
        return Task.CompletedTask;
    }

    public Task TellServerToDisconnectAsync(ServerId id, CancellationToken cts)
    {
        _coordinator.Tell(new ServerDisconnect(id));
        return Task.CompletedTask;
    }

    public Task RemoveServerAsync(ServerId id, CancellationToken cts)
    {
        _coordinator.Tell(new ServerRemove(id));
        return Task.CompletedTask;
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