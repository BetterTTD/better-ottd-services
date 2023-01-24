using Common;
using Domain.Models;
using Domain.ValueObjects;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Error(ServerId Id, ServerCredentials Credentials) : Model(Id, Credentials)
    {
        public required Exception Exception { get; init; }
        public required string Message { get; init; } = "Unknown error";
    };

    private sealed record ErrorOccurred;

    private State<State, Model> ErrorHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (Error model, _) => F.Run(() =>
        {
            _logger.Error(model.Exception, model.Message);

            return Stay();
        }),

        _ => throw new InvalidOperationException()
    };
}