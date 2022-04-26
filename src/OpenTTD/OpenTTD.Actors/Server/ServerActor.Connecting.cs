namespace OpenTTD.Actors.Server;

public sealed record Connecting : Model;

public sealed partial class ServerActor
{
    private State<State, Model> ConnectingHandler(Event<Model> @event)
    {
        if (@event.StateData is not Connecting connecting)
        {
            return GoTo(State.Error).Using(new Error());
        }

        return Stay();
    }
}