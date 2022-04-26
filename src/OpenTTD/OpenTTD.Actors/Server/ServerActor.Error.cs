namespace OpenTTD.Actors.Server;

public sealed record Error : Model
{
    public Exception Exception { get; init; } = null!;
    public string Message { get; init; } = "Unknown error";
};

public sealed partial class ServerActor
{
    private State<State, Model> ErrorHandler(Event<Model> @event)
    {
        if (@event.StateData is not Error error)
        {
            throw new InvalidOperationException();
        }
        
        _logger.Error(error.Exception, error.Message);

        return Stay();
    }
}