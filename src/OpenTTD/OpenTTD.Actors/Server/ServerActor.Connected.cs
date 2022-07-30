using Common;
using OpenTTD.Actors.Receiver;
using Domain.Models;
using Domain.ValueObjects;
using Networking.Enums;
using Networking.Messages.Inbound;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connected(
        ServerId Id, 
        ServerCredentials Credentials, 
        NetworkActors Network) : NetworkModel(Id, Credentials, Network);

    private State<State, Model> ConnectedHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (Connected model, ReceivedMsg msg) => F.Run(() =>
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
                //Grace shutdown
                return GoTo(State.IDLE).Using(new Idle(model.Id, model.Credentials));
            }
            
            return Stay().Using(model);
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