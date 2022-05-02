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
using Common;
using OpenTTD.Domain.Models;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Idle(ServerCredentials Credentials) : Model(Credentials);
    
    private sealed record ConnectionEstablished;

    private State<State, Model> IdleHandler(Event<Model> @event) => (@event.FsmEvent, @event.StateData) switch
    {
        (Connect, Idle(var credentials)) => F.Run(() =>
        {
            Task.Run(async () =>
                {
                    try
                    {
                        var address = credentials.NetworkAddress;
                        await _client.ConnectAsync(address.IpAddress, address.Port);
                        return Result.Success(new ConnectionEstablished());
                    }
                    catch (SocketException exn)
                    {
                        return Result.Failure<ConnectionEstablished>(exn);
                    }
                })
                .PipeTo(Self, Sender);

            return Stay();
        }),

        (Result<ConnectionEstablished> result, Idle(var credentials)) => F.Run(() =>
        {
            if (!result.IsSuccess)
            {
                Self.Tell(new ErrorOccurred(), Sender);

                return GoTo(State.ERROR).Using(new Error(credentials)
                {
                    Exception = result.Exception,
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

            return GoTo(State.CONNECTING).Using(connectingState);
        }),

        var (_, (credentials)) => F.Run(() =>
        {
            Self.Tell(new ErrorOccurred(), Sender);

            return GoTo(State.ERROR).Using(new Error(credentials)
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            });
        }),
        
        _ => throw new InvalidOperationException()
    };
}