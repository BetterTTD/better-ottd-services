using Common;
using OpenTTD.Domain.Models;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Error(ServerCredentials Credentials) : Model(Credentials)
    {
        public Exception Exception { get; init; } = null!;
        public string Message { get; init; } = "Unknown error";
    };

    private sealed record ErrorOccurred;

    private State<State, Model> ErrorHandler(Event<Model> @event) => (@event.FsmEvent, @event.StateData) switch
    {
        (ErrorOccurred, Error model) => F.Run(() =>
        {
            _logger.Error(model.Exception, model.Message);

            return Stay();
        }),

        _ => throw new InvalidOperationException()
    };
}