using Akka.Actor;
using Akka.Util;
using Akka.Util.Internal;
using Common;
using OpenTTD.Actors.Receiver;
using OpenTTD.Actors.Sender;
using OpenTTD.Domain;
using OpenTTD.Networking.Enums;
using OpenTTD.Networking.Messages;
using OpenTTD.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.Networking.Messages.Outbound.Poll;
using OpenTTD.Networking.Messages.Outbound.UpdateFrequency;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connecting(
        ServerCredentials Credentials,
        NetworkActors Network,
        Option<ServerProtocolMessage> MaybeProtocol,
        Option<ServerWelcomeMessage> MaybeWelcome) : NetworkModel(Network);

    private State<State, Model> ConnectingHandler(Event<Model> @event) => (@event.FsmEvent, @event.StateData) switch
    {
        (ReceivedMsg msg, Connecting model) => F.Run(() =>
        {
            var state = msg.Message switch
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
                .Union(new Dictionary<AdminUpdateType, uint>
                {
                    { AdminUpdateType.ADMIN_UPDATE_COMPANY_INFO, uint.MaxValue },
                    { AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO, uint.MaxValue }
                }.Select(x => new PollMessage(x.Key, x.Value)))
                .Union(new Dictionary<AdminUpdateType, UpdateFrequency>
                {
                    { AdminUpdateType.ADMIN_UPDATE_CHAT, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                    { AdminUpdateType.ADMIN_UPDATE_COMPANY_INFO, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC }
                }.Select(x => new UpdateFrequencyMessage(x.Key, x.Value)))
                .Select(x => new SendMessage(x))
                .ForEach(x => state.Network.Sender.Tell(x));

            var protocol = state.MaybeProtocol.Value;
            var welcome = state.MaybeWelcome.Value;

            var server = new Domain.Server
            {
                Name = welcome.ServerName,
                IsDedicated = welcome.IsDedicated,
                Date = welcome.CurrentDate,
                Map = new Map
                {
                    Name = welcome.MapName,
                    Landscape = welcome.Landscape,
                    Width = welcome.MapWidth,
                    Height = welcome.MapHeight
                },
                Network = new ServerNetwork
                {
                    Version = protocol.NetworkVersion,
                    Revision = welcome.NetworkRevision,
                    UpdateFrequencies = protocol.AdminUpdateSettings,
                },
                Companies = new List<Company> { Company.Spectator }
            };

            return GoTo(State.Connected).Using(new Connected(state.Credentials, state.Network, server));
        }),

        _ => F.Run(() =>
        {
            Self.Tell(new ErrorOccurred(), Sender);

            return GoTo(State.Error).Using(new Error
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            });
        })
    };
}