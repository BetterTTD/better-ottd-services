namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Error : Model
    {
        public Exception Exception { get; init; } = null!;
        public string Message { get; init; } = "Unknown error";
    };

    private sealed record ErrorOccurred;
    
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