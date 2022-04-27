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

public sealed record Idle(ServerCredentials Credentials) : Model;

public sealed partial class ServerActor
{
    private State<State, Model> IdleHandler(Event<Model> @event)
    {
        if (@event.StateData is not Idle(var credentials))
        {
            return GoTo(State.Error).Using(new Error());
        }

        if (@event.FsmEvent is Connect)
        {
            try
            {
                var client = new TcpClient();
                client.ConnectAsync(credentials.ServerAddress.IpAddress, credentials.ServerAddress.Port);

                var stream = client.GetStream();
                
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
            catch (Exception exn)
            {
                return GoTo(State.Error).Using(new Error { Exception = exn, Message = exn.Message });
            }
        }

        return null!;
    }
}