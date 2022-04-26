using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Util;
using OpenTTD.Actors.Server;
using OpenTTD.Domain;

namespace OpenTTD.Actors.Coordinator;

public record ServerAdd(ServerCredentials Credentials);
public record ServerConnect(Guid Guid);
public record ServerDisconnect(Guid Guid);
public record ServerRemove(Guid Guid);

public record ServerAdded(Guid ServerIdentifier);

public class CoordinatorActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    public CoordinatorActor()
    {
        var servers = new Dictionary<Guid, (ServerCredentials Credentials, IActorRef Ref)>();

        Receive<ServerAdd>(msg =>
        {
            if (servers.All(pair => pair.Value.Credentials != msg.Credentials))
            {
                var serverProps = DependencyResolver.For(Context.System).Props<ServerActor>(msg.Credentials);
                var serverRef = Context.ActorOf(serverProps);
                var guid = Guid.NewGuid();
                
                servers.Add(guid, (msg.Credentials, serverRef));
            
                _logger.Warning($"Server added: {msg.Credentials.ServerAddress}");

                Sender.Tell(Result.Success(new ServerAdded(guid)));
                return;
            }
            
            _logger.Warning($"Server already exists: {msg.Credentials.ServerAddress}");
            
            Sender.Tell(Result.Failure<ServerAdded>(new ArgumentException("Server already exists")));
        });

        Receive<ServerConnect>(msg =>
        {
            if (servers.TryGetValue(msg.Guid, out var data)) data.Ref.Tell(new Connect());
        });
    }
}