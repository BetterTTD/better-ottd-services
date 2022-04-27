using System.Net.Sockets;
using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using OpenTTD.Domain;

namespace OpenTTD.Actors.Server;

public sealed record NetworkActors(IActorRef Sender, IActorRef Receiver);

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

    private readonly TcpClient _client = new();

    public ServerActor(ServerCredentials credentials)
    {
        StartWith(State.Idle, new Idle(credentials));
        
        When(State.Idle, IdleHandler);
        
        When(State.Connecting, ConnectingHandler);
        When(State.Connected, ConnectedHandler);
        When(State.Error, ErrorHandler);
        
        OnTransition((_, next) =>
        {
            if (next == State.Error)
            {
                Self.Tell(new ErrorOccurred(), Sender);
            }   
        });
        
        Initialize();
    }
}