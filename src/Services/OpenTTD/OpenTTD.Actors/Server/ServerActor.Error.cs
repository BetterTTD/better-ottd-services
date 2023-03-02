using Akka.Actor;
using Common;
using OpenTTD.Domain.Models;
using OpenTTD.Domain.ValueObjects;

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
        (Error(var id, var credentials), Connect) => F.Run(() =>
        {
            TryToConnect(id, credentials);
            return GoTo(State.IDLE).Using(new Idle(id, credentials));
        }),

        (Error model, _) => F.Run(() =>
        {
            _logger.Error(model.Exception, $"[ServerId:{model.Id.Value}] {model.Message}");

            Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    return new Connect();
                })
                .PipeTo(Self, Sender);
            
            return Stay();
        }),

        _ => throw new InvalidOperationException()
    };
}