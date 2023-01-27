using System.Net.Sockets;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Util;
using OpenTTD.Actors.Receiver;
using OpenTTD.Actors.Sender;
using Common;
using Domain.Models;
using Domain.ValueObjects;
using OpenTTD.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.Networking.Messages.Outbound.Join;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Idle(ServerId Id, ServerCredentials Credentials) : Model(Id, Credentials);
    
    private sealed record ConnectionEstablished;

    private State<State, Model> IdleHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (Idle(var serverId, var credentials), Connect) => F.Run(() =>
        {
            Task.Run(async () =>
                {
                    try
                    {
                        var address = credentials.NetworkAddress;
                        _logger.Info("[{Guid}] Establishing connection with address {Address}", serverId.Value, address);
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

        (Idle(var serverId, var credentials), Result<ConnectionEstablished> result) => F.Run(() =>
        {
            if (!result.IsSuccess)
            {
                Self.Tell(new ErrorOccurred(), Sender);

                return GoTo(State.ERROR).Using(new Error(serverId, credentials)
                {
                    Exception = result.Exception,
                    Message = "Connection could not be established"
                });
            }
            
            _logger.Info("[{ServerId}] Connection established successfully", serverId.Value);

            var stream = _client.GetStream();

            var senderProps = DependencyResolver
                .For(Context.System)
                .Props<SenderActor>(serverId, stream);
            var receiverProps = DependencyResolver
                .For(Context.System)
                .Props<ReceiverActor>(serverId, stream);

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
                serverId, credentials, network,
                Option<ServerProtocolMessage>.None,
                Option<ServerWelcomeMessage>.None);

            return GoTo(State.CONNECTING).Using(connectingState);
        }),

        var ((id, credentials), _) => F.Run(() =>
        {
            Self.Tell(new ErrorOccurred(), Sender);

            return GoTo(State.ERROR).Using(new Error(id, credentials)
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            });
        }),
        
        _ => throw new InvalidOperationException()
    };
}