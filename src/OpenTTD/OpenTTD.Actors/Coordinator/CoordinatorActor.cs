using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Util;
using OpenTTD.Actors.Server;
using OpenTTD.Domain;
using OpenTTD.Domain.Models;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Actors.Coordinator;

public record ServerAdd(ServerCredentials Credentials);
public record ServerConnect(ServerId Id);
public record ServerDisconnect(ServerId Id);
public record ServerRemove(ServerId Id);

public record ServerAdded(ServerId Id);

public class CoordinatorActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    public CoordinatorActor()
    {
        var servers = new Dictionary<ServerId, (ServerCredentials Credentials, IActorRef Ref)>();

        Receive<ServerAdd>(msg =>
        {
            if (servers.All(pair => pair.Value.Credentials != msg.Credentials))
            {
                var serverProps = DependencyResolver.For(Context.System).Props<ServerActor>(msg.Credentials);
                var serverRef = Context.ActorOf(serverProps);
                var id = new ServerId(Guid.NewGuid());
                
                servers.Add(id, (msg.Credentials, serverRef));
            
                _logger.Warning($"Server added: {msg.Credentials.NetworkAddress}");

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