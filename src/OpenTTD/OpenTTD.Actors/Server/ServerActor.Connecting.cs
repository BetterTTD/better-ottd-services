using Akka.Actor;
using Akka.Util;
using Akka.Util.Internal;
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
        Option<ServerWelcomeMessage> MaybeWelcome) : Model;
    
    private State<State, Model> ConnectingHandler(Event<Model> @event)
    {
        if (@event.StateData is not Connecting connecting)
        {
            return GoTo(State.Error).Using(new Error
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            });
        }

        if (@event.FsmEvent is ReceivedMsg msg)
        {
            try
            {
                var state = msg.Message switch
                {
                    ServerProtocolMessage protocolMsg => connecting with
                    {
                        MaybeProtocol = new Option<ServerProtocolMessage>(protocolMsg)
                    },
                    ServerWelcomeMessage welcomeMsg => connecting with
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

            }
            catch (Exception exn)
            {
                return GoTo(State.Error).Using(new Error { Exception = exn, Message = exn.Message });
            }
        }

        return null!;
    }
}