namespace OpenTTD.Actors.Server;

public sealed record Idle : Model;

public sealed partial class ServerActor
{
    private State<State, Model> IdleHandler(Event<Model> @event)
    {
        if (@event.StateData is not Idle idle)
        {
            return GoTo(State.Error).Using(new Error());
        }

        return Stay();
    }

}