using Akka.Actor;
using Common;
using OpenTTD.Domain.Events;
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
        (Error model, Reconnect) => F.Run(() =>
        {
            Self.Tell(new Connect());
            
            return GoTo(State.CONNECTING).Using(new InitialConnecting(model.Id, model.Credentials));
        }),
        
        (Error model, _) => F.Run(() =>
        {
            _logger.Error(model.Exception, $"[{nameof(ServerActor)}] [ServerId:{model.Id.Value}] {model.Message}");

            _mediator.Publish(new ServerError(model.Id, model.Exception, model.Message));

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                return new Reconnect();
            }).PipeTo(Self, Sender);
            
            return Stay();
        }),

        _ => throw new InvalidOperationException()
    };
}