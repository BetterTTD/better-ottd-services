using Akka.Actor;
using Akka.Util;
using Akka.Util.Internal;
using Common;
using OpenTTD.Actors.Receiver;
using OpenTTD.Actors.Sender;
using Domain.Models;
using Domain.ValueObjects;
using OpenTTD.Networking.Enums;
using OpenTTD.Networking.Messages;
using OpenTTD.Networking.Messages.Inbound;
using OpenTTD.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.Networking.Messages.Outbound.Poll;
using OpenTTD.Networking.Messages.Outbound.UpdateFrequency;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connecting(
        ServerId Id, 
        ServerCredentials Credentials,
        NetworkActors Network,
        Option<ServerProtocolMessage> MaybeProtocol,
        Option<ServerWelcomeMessage> MaybeWelcome) : NetworkModel(Id, Credentials, Network);

    private State<State, Model> ConnectingHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (Connecting model, ReceivedMsg msg) => F.Run(() =>
        {
            var result = msg.MsgResult;
            if (!result.IsSuccess)
            {
                return GoTo(State.ERROR).Using(new Error(model.Id, model.Credentials)
                {
                    Exception = result.Exception,
                    Message = result.Exception.Message
                });
            }
            
            if (msg.MsgResult.Value is GenericMessage { PacketType: PacketType.ADMIN_PACKET_SERVER_SHUTDOWN })
            {
                return GoTo(State.IDLE).Using(new Idle(model.Id, model.Credentials));
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
                    { UpdateType.ADMIN_UPDATE_CMD_LOGGING , UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC},
                    { UpdateType.ADMIN_UPDATE_CONSOLE, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { UpdateType.ADMIN_UPDATE_CHAT, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { UpdateType.ADMIN_UPDATE_CLIENT_INFO, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { UpdateType.ADMIN_UPDATE_COMPANY_INFO, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC }
                }.Select(x => new UpdateFrequencyMessage(x.Key, x.Value)))
                .Select(x => new SendMessage(x))
                .ForEach(x => state.Network.Sender.Tell(x));

            var server = _dispatcher.Create(model.Id, state.MaybeWelcome.Value, model.MaybeProtocol.Value);
            
            return GoTo(State.CONNECTED).Using(new Connected(state.Id, state.Credentials, state.Network, server));
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