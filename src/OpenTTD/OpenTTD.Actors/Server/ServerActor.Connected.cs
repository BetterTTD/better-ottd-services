using OpenTTD.Actors.Receiver;
using OpenTTD.Domain;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connected(
        ServerCredentials StateCredentials, 
        NetworkActors StateNetwork, 
        Domain.Server Server) : Model;
    
    private State<State, Model> ConnectedHandler(Event<Model> @event)
    {
        if (@event.StateData is not Connected connected)
        {
            return GoTo(State.Error).Using(new Error
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            });
        }

        if (@event.FsmEvent is ReceivedMsg msg)
        {
            
        }
        
        return Stay();
    }
}