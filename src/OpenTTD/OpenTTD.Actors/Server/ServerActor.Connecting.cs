using Akka.Actor;
using Akka.Util;
using OpenTTD.Actors.Receiver;
using OpenTTD.Actors.Sender;
using OpenTTD.Domain;
using OpenTTD.Networking.Enums;
using OpenTTD.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.Networking.Messages.Outbound.UpdateFrequency;

namespace OpenTTD.Actors.Server;

public sealed record Connecting(
    ServerCredentials Credentials, 
    NetworkActors Network,
    Option<ServerProtocolMessage> MaybeProtocol,
    Option<ServerWelcomeMessage> MaybeWelcome) : Model;

public sealed partial class ServerActor
{
    private State<State, Model> ConnectingHandler(Event<Model> @event)
    {
        if (@event.StateData is not Connecting connecting)
        {
            return GoTo(State.Error).Using(new Error());
        }

        if (@event.FsmEvent is ReceivedMsg msg)
        {
            try
            {
                var state = msg.Message switch
                {
                    ServerProtocolMessage protocol => connecting with
                    {
                        MaybeProtocol = new Option<ServerProtocolMessage>(protocol)
                    },
                    ServerWelcomeMessage welcome => connecting with
                    {
                        MaybeWelcome = new Option<ServerWelcomeMessage>(welcome)
                    },
                    _ => throw new InvalidCastException()
                };

                if (state.MaybeProtocol.HasValue && state.MaybeWelcome.HasValue)
                {
                    var polls = new Dictionary<AdminUpdateType, UpdateFrequency>
                    {
                        { AdminUpdateType.ADMIN_UPDATE_CHAT, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                        { AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC },
                        { AdminUpdateType.ADMIN_UPDATE_COMPANY_INFO, UpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC }
                    };

                    foreach (var (type, frequency) in polls)
                    {
                        state.Network.Sender.Tell(new SendMessage(new UpdateFrequencyMessage
                        {
                            UpdateType = type,
                            UpdateFrequency = frequency
                        }));
                    }
                    
                    return GoTo(State.Connected).Using(new Connected());
                }
                
                return Stay().Using(state);
            }
            catch (Exception exn)
            {
                return GoTo(State.Error).Using(new Error { Exception = exn, Message = exn.Message });
            }
        }

        return null!;
    }
}