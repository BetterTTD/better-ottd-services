namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connected : Model;
    
    private State<State, Model> ConnectedHandler(Event<Model> @event)
    {
        if (@event.StateData is not Connected connected)
        {
            return GoTo(State.Error).Using(new Error());
        }

        return Stay();
    }
}