using Akka.Actor;
using Akka.Event;
using Common;
using OpenTTD.AdminClient.Domain.Events;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Error(ServerId Id, ServerNetwork Network) : Model(Id, Network)
    {
        public required Exception Exception { get; init; }
        public required string Message { get; init; } = "Unknown error";
    }

    private sealed record ErrorOccurred;

    private State<State, Model> ErrorHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (Error model, Reconnect) => F.Run(() =>
        {
            Self.Tell(new Connect());
            
            return GoTo(State.CONNECTING).Using(new InitialConnecting(model.Id, model.Network));
        }),
        
        (Error model, _) => F.Run(() =>
        {
            _logger.Error(model.Exception, $"[{nameof(ServerActor)}] [ServerId:{model.Id.Value}] {model.Message}");

            _mediator.Publish(new ServerError(model.Id, model.Exception, model.Message));

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                return new Reconnect();
            }).PipeTo(Self, Self);
            
            return Stay();
        }),

        _ => throw new InvalidOperationException()
    };
}