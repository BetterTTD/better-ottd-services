using System.Net.Sockets;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Util;
using Akka.Util.Internal;
using Common;
using MediatR;
using OpenTTD.AdminClient.Domain.ValueObjects;
using OpenTTD.AdminClient.Networking.Enums;
using OpenTTD.AdminClient.Networking.Messages;
using OpenTTD.AdminClient.Networking.Messages.Inbound;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.AdminClient.Networking.Messages.Outbound.Join;
using OpenTTD.AdminClient.Networking.Messages.Outbound.Poll;
using OpenTTD.AdminClient.Networking.Messages.Outbound.UpdateFrequency;

namespace OpenTTD.AdminClient.Actors;

public sealed partial class ServerActor
{
    
    private sealed record InitialConnecting(ServerId Id, ServerNetwork Network) : Model(Id, Network);

    private sealed record Connecting(
        ServerId Id,
        ServerNetwork Network,
        NetworkActors NetworkActors,
        Option<ServerProtocolMessage> MaybeProtocol,
        Option<ServerWelcomeMessage> MaybeWelcome) : NetworkModel(Id, Network, NetworkActors);

    private State<State, Model> ConnectingHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (InitialConnecting (var serverId, var networkInfo), Connect) => F.Run(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    var address = networkInfo.NetworkAddress;

                    _logger.Debug(
                        "[{Actor}] [ServerId:{ServerId}] Establishing connection with address {Address}",
                        nameof(ServerActor), serverId.Value, address);

                    await _client.ConnectAsync(address.IpAddress, address.Port.Value);

                    return Result.Success(Unit.Value);
                }
                catch (SocketException exn)
                {
                    return Result.Failure<Unit>(exn);
                }
            }).PipeTo(Self, Self);

            return Stay();
        }),

        (InitialConnecting (var serverId, var credentials), Result<Unit> result) => F.Run(() =>
        {
            if (!result.IsSuccess)
            {
                return GoTo(State.ERROR).Using(new Error(serverId, credentials)
                {
                    Exception = result.Exception,
                    Message = $"Connection could not be established with address: {credentials.NetworkAddress}"
                });
            }

            _logger.Debug(
                "[{Actor}] [ServerId:{ServerId}] Connection with address {Address} established successfully",
                nameof(ServerActor), serverId.Value, credentials.NetworkAddress);

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

            network.Sender.Tell(new SendNetworkMessage(new JoinMessage
            {
                AdminName = credentials.AdminName.Value,
                AdminVersion = credentials.Version.Value,
                Password = credentials.Password.Value
            }));

            var connectingState = new Connecting(
                serverId, credentials, network,
                Option<ServerProtocolMessage>.None,
                Option<ServerWelcomeMessage>.None);

            return GoTo(State.CONNECTING).Using(connectingState);
        }),

        (Connecting model, ReceivedNetworkMessage msg) => F.Run(() =>
        {
            var result = msg.MsgResult;
            if (!result.IsSuccess)
            {
                return GoTo(State.ERROR).Using(new Error(model.Id, model.Network)
                {
                    Exception = result.Exception,
                    Message = result.Exception.Message
                });
            }

            if (msg.MsgResult.Value is GenericMessage { PacketType: PacketType.ADMIN_PACKET_SERVER_SHUTDOWN })
            {
                return GoTo(State.IDLE).Using(new Idle(model.Id, model.Network));
            }

            var state = result.Value switch
            {
                ServerProtocolMessage protocolMsg => model with
                {
                    MaybeProtocol = new Option<ServerProtocolMessage>(protocolMsg)
                },
                ServerWelcomeMessage welcomeMsg => model with
                {
                    MaybeWelcome = new Option<ServerWelcomeMessage>(welcomeMsg)
                },
                _ => throw new InvalidCastException()
            };

            if (!state.MaybeProtocol.HasValue || !state.MaybeWelcome.HasValue)
            {
                return Stay().Using(state);
            }

            new List<IMessage>()
                .Union(new Dictionary<UpdateType, uint>
                {
                    { UpdateType.ADMIN_UPDATE_COMPANY_INFO, uint.MaxValue },
                    { UpdateType.ADMIN_UPDATE_CLIENT_INFO, uint.MaxValue }
                }.Select(x => new PollMessage(x.Key, x.Value)))
                .Union(new Dictionary<UpdateType, UpdateFrequency>
                {
                    { UpdateType.ADMIN_UPDATE_CMD_LOGGING, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { UpdateType.ADMIN_UPDATE_CONSOLE, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { UpdateType.ADMIN_UPDATE_CHAT, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { UpdateType.ADMIN_UPDATE_CLIENT_INFO, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { UpdateType.ADMIN_UPDATE_COMPANY_INFO, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC }
                }.Select(x => new UpdateFrequencyMessage(x.Key, x.Value)))
                .Select(x => new SendNetworkMessage(x))
                .ForEach(x => state.NetworkActors.Sender.Tell(x));

            return GoTo(State.CONNECTED).Using(new Connected(state.Id, state.Network, state.NetworkActors));
        }),

        var ((id, credentials), _) => F.Run(() =>
            GoTo(State.ERROR).Using(new Error(id, credentials)
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            })),

        _ => throw new InvalidOperationException()
    };
}