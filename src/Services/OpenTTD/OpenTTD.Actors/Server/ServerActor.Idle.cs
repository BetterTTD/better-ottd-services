using Akka.Actor;
using Common;
using OpenTTD.Actors.Receiver;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Idle(ServerId Id, ServerNetwork Network) : Model(Id, Network);

    private State<State, Model> IdleHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (Idle (var id, var credentials), Connect cmd) => F.Run(() =>
        {
            Self.Tell(cmd);
            return GoTo(State.CONNECTING).Using(new InitialConnecting(id, credentials));
        }),

        (Idle model, ReceivedMsg) => F.Run(() => Stay().Using(model)),

        var ((id, credentials), _) => F.Run(() => GoTo(State.ERROR).Using(new Error(id, credentials)
        {
            Exception = new InvalidOperationException(),
            Message = "Invalid state data"
        })),

        _ => throw new InvalidOperationException()
    };
}