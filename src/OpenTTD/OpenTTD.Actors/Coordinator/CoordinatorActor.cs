using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Util;
using OpenTTD.Actors.Server;
using Domain.Models;
using Domain.ValueObjects;

namespace OpenTTD.Actors.Coordinator;

public sealed record ServerAdd(ServerCredentials Credentials);
public sealed record ServerConnect(ServerId Id);
public sealed record ServerDisconnect(ServerId Id);
public sealed record ServerRemove(ServerId Id);

public sealed record ServerAdded(ServerId Id);

public sealed class CoordinatorActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    public CoordinatorActor()
    {
        var servers = new Dictionary<ServerId, (ServerCredentials Credentials, IActorRef Ref)>();

        Receive<ServerAdd>(msg =>
        {
            if (servers.All(pair => pair.Value.Credentials != msg.Credentials))
            {
                var id = new ServerId(Guid.NewGuid());
                var serverProps = DependencyResolver.For(Context.System).Props<ServerActor>(id, msg.Credentials);
                var serverRef = Context.ActorOf(serverProps);
                
                servers.Add(id, (msg.Credentials, serverRef));
            
                _logger.Info($"Server added: {msg.Credentials.NetworkAddress}");

                Sender.Tell(Result.Success(new ServerAdded(id)));
                return;
            }
            
            _logger.Warning($"Server already exists: {msg.Credentials.NetworkAddress}");
            
            Sender.Tell(Result.Failure<ServerAdded>(new ArgumentException("Server already exists")));
        });

        Receive<ServerConnect>(msg =>
        {
            if (servers.TryGetValue(msg.Id, out var data))
            {
                data.Ref.Tell(new Connect());
            }
        });
        
        Receive<ServerDisconnect>(msg =>
        {
            if (servers.TryGetValue(msg.Id, out var data))
            {
                data.Ref.Tell(new Disconnect());
            }
        });
        
        Receive<ServerRemove>(msg =>
        {
            if (servers.TryGetValue(msg.Id, out var data))
            {
                data.Ref.Tell(PoisonPill.Instance);
                servers.Remove(msg.Id);
            }
        });
    }
}