using Common;
using OpenTTD.AdminClient.Domain.ValueObjects;
using OpenTTD.AdminClient.Networking.Enums;
using OpenTTD.AdminClient.Networking.Messages.Inbound;

namespace OpenTTD.AdminClient.Actors;

public sealed partial class ServerActor
{
    private sealed record Connected(
        ServerId Id,
        ServerNetwork ServerNetwork, 
        NetworkActors NetworkActors) : NetworkModel(Id, ServerNetwork, NetworkActors);

    private State<State, Model> ConnectedHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (Connected model, ReceivedMsg msg) => F.Run(() =>
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

            return Stay().Using(model);
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