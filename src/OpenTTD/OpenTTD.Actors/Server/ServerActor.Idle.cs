using System.Net.Sockets;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Util;
using OpenTTD.Actors.Receiver;
using OpenTTD.Actors.Sender;
using OpenTTD.Domain;
using OpenTTD.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.Networking.Messages.Outbound.Join;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Idle(ServerCredentials Credentials) : Model;
    
    private sealed record ConnectionEstablished;
    
    private State<State, Model> IdleHandler(Event<Model> @event)
    {
        if (@event.StateData is not Idle(var credentials))
        {
            return GoTo(State.Error).Using(new Error
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            });
        }

        if (@event.FsmEvent is Connect)
        {
            EstablishConnection(credentials.ServerAddress);

            return Stay();
        }

        if (@event.FsmEvent is Result<ConnectionEstablished> connectionResult)
        {
            if (!connectionResult.IsSuccess)
            {
                return GoTo(State.Error).Using(new Error
                {
                    Exception = connectionResult.Exception,
                    Message = "Connection could not be established"
                });
            }
            
            var stream = _client.GetStream();
            
            var senderProps = DependencyResolver
                .For(Context.System)
                .Props<SenderActor>(stream);
            var receiverProps = DependencyResolver
                .For(Context.System)
                .Props<ReceiverActor>(stream);

            var network = new NetworkActors(
                Sender: Context.ActorOf(senderProps),
                Receiver: Context.ActorOf(receiverProps));

            network.Sender.Tell(new SendMessage(new JoinMessage
            {
                AdminName = credentials.Name,
                AdminVersion = credentials.Version,
                Password = credentials.Password
            }));

            var connectingState = new Connecting(
                credentials, network,
                Option<ServerProtocolMessage>.None,
                Option<ServerWelcomeMessage>.None);
            
            return GoTo(State.Connecting).Using(connectingState);
        }
        
        return null!;
    }

    private void EstablishConnection(ServerAddress address)
    {
        Task.Run(async () =>
            {
                try
                {
                    await _client.ConnectAsync(address.IpAddress, address.Port);
                    return Result.Success(new ConnectionEstablished());
                }
                catch (SocketException exn)
                {
                    return Result.Failure<ConnectionEstablished>(exn);
                }
            })
            .PipeTo(Self, Sender);
    }
}