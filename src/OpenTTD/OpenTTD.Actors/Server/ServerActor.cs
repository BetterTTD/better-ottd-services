using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;

namespace OpenTTD.Actors.Server;

public enum State
{
    Idle,
    Connecting,
    Connected,
    Error
}

public abstract record Model;

public sealed partial class ServerActor : FSM<State, Model>
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    public ServerActor()
    {
        StartWith(State.Idle, new Idle());
        
        When(State.Idle, IdleHandler);
        When(State.Connecting, ConnectingHandler);
        When(State.Connected, ConnectedHandler);
        When(State.Error, ErrorHandler);
    }
}