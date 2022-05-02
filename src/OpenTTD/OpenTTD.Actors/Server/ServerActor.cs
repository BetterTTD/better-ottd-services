using System.Net.Sockets;
using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using Common;
using OpenTTD.Domain;
using OpenTTD.Domain.Models;

namespace OpenTTD.Actors.Server;

public sealed record NetworkActors(IActorRef Sender, IActorRef Receiver);

public enum State
{
    IDLE,
    CONNECTING,
    CONNECTED,
    ERROR
}

public abstract record Model(ServerCredentials Credentials);
public abstract record NetworkModel(
    ServerCredentials Credentials, 
    NetworkActors Network) : Model(Credentials);

public sealed partial class ServerActor : FSM<State, Model>
{
    private readonly IServerDispatcher _dispatcher;
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    private readonly TcpClient _client = new();

    public ServerActor(ServerCredentials credentials, IServerDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        
        StartWith(State.IDLE, new Idle(credentials));
        
        When(State.IDLE, IdleHandler);
        When(State.CONNECTING, ConnectingHandler);
        When(State.CONNECTED, ConnectedHandler);
        When(State.ERROR, ErrorHandler);
        
        WhenUnhandled(DefaultHandler);
        
        OnTransition((prev, next) =>
        {
            if (prev == State.IDLE && next == State.IDLE)
            {
                return;
            }
            
            if (next is State.ERROR or State.IDLE && StateData is NetworkModel model)
            {
                model.Network.Receiver.Tell(PoisonPill.Instance);
                model.Network.Sender.Tell(PoisonPill.Instance);
            }

            if (next is State.ERROR)
            {
                Self.Tell(new ErrorOccurred(), Sender);
            }
        });
        
        Initialize();
    }

    private State<State, Model> DefaultHandler(Event<Model> @event) => (@event.FsmEvent, @event.StateData) switch
    {
        (Disconnect, { } model) => F.Run(() => 
            GoTo(State.IDLE).Using(new Idle(model.Credentials))),

        var (_, (credentials)) => F.Run(() =>
        {
            Self.Tell(new ErrorOccurred(), Sender);

            return GoTo(State.ERROR).Using(new Error(credentials)
            {
                Exception = new InvalidOperationException(),
                Message = $"{nameof(DefaultHandler)} Unhandled message"
            });
        }),
        
        _ => throw new InvalidOperationException()
    };
}