namespace OpenTTD.Actors.Server;

public sealed record Error : Model;

public sealed partial class ServerActor
{
    private State<State, Model> ErrorHandler(Event<Model> @event)
    {
        if (@event.StateData is not Error error)
        {
            throw new InvalidOperationException();
        }

        return Stay();
    }
}