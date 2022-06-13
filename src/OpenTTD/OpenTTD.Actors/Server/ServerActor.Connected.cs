using Common;
using OpenTTD.Actors.Receiver;
using Domain.Models;
using Domain.ValueObjects;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connected(
        ServerId Id, 
        ServerCredentials Credentials, 
        NetworkActors Network, 
        Domain.Entities.Server Server) : NetworkModel(Id, Credentials, Network);

    private State<State, Model> ConnectedHandler(Event<Model> @event) => (@event.FsmEvent, @event.StateData) switch
    {
        (ReceivedMsg msg, Connected model) => F.Run(() =>
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

            var server = _dispatcher.Dispatch(result.Value, model.Server);
            
            return Stay().Using(model with { Server = server });
        }),
        
        var (_, (id, credentials)) => F.Run(() =>
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