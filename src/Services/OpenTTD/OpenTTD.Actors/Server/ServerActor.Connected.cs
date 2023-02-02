using Common;
using OpenTTD.Actors.Receiver;
using OpenTTD.Domain.Models;
using OpenTTD.Domain.ValueObjects;
using OpenTTD.Networking.Messages.Inbound;
using OpenTTD.Networking.Enums;
using OpenTTD.Networking.Messages.Inbound.ServerChat;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connected(
        ServerId Id,
        ServerCredentials Credentials, 
        NetworkActors Network,
        global::OpenTTD.Domain.Entities.Server Server) : NetworkModel(Id, Credentials, Network);

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
                return GoTo(State.IDLE).Using(new Idle(model.Id, model.Credentials));
            }

            model = model with { Server = _dispatcher.Dispatch(msg.MsgResult.Value, model.Server) };

            if (msg.MsgResult.Value is ServerChatMessage chatMsg)
            {
                var client = model.Server.Companies
                    .SelectMany(c => c.Clients)
                    .First(cl => cl.Id == new ClientId(chatMsg.ClientId));
                
                _logger.Debug(
                    "[{ServerId}] [{MessageType}] {ClientName}: {ClientMessage}", 
                    model.Id.Value, nameof(ServerChatMessage), client.Name, chatMsg.Message);
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