using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Util;
using OpenTTD.Actors.Server;
using OpenTTD.Domain;

namespace OpenTTD.Actors.Coordinator;

public record AddServer(ServerCredentials Credentials);

public record ServerAdded(Guid ServerIdentifier);

public class CoordinatorActor : ReceiveActor
{
    public CoordinatorActor()
    {
        var servers = new Dictionary<Guid, (ServerCredentials Credentials, IActorRef Ref)>();

        Receive<AddServer>(msg =>
        {
            if (servers.All(pair => pair.Value.Credentials != msg.Credentials))
            {
                var serverProps = DependencyResolver.For(Context.System).Props<ServerActor>(msg.Credentials);
                var serverRef = Context.ActorOf(serverProps);
                var guid = Guid.NewGuid();
                
                servers.Add(guid, (msg.Credentials, serverRef));
                
                Sender.Tell(Result.Success(new ServerAdded(guid)));
            }
            
            Sender.Tell(Result.Failure<ServerAdded>(new ArgumentException("Server already exists")));
        });
    }
}