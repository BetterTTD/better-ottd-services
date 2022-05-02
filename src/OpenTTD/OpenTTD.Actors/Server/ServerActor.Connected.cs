using Common;
using OpenTTD.Actors.Receiver;
using OpenTTD.Domain.Models;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connected(
        ServerCredentials Credentials, 
        NetworkActors Network, 
        Domain.Entities.Server Server) : NetworkModel(Credentials, Network);

    private State<State, Model> ConnectedHandler(Event<Model> @event) => (@event.FsmEvent, @event.StateData) switch
    {
        (ReceivedMsg msg, Connected model) => F.Run(() =>
        {
            var result = msg.MsgResult;
            if (!result.IsSuccess)
            {
                return GoTo(State.ERROR).Using(new Error(model.Credentials)
                {
                    Exception = result.Exception,
                    Message = result.Exception.Message
                });
            }

            return Stay();
        }),
        
        var (_, (credentials)) => F.Run(() =>
        {
            Self.Tell(new ErrorOccurred(), Sender);

            return GoTo(State.ERROR).Using(new Error(credentials)
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            });
        }),
        
        _ => throw new InvalidOperationException()
    };
}