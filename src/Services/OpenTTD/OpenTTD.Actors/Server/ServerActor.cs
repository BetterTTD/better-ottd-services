using System.Net.Sockets;
using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using Common;
using MediatR;
using OpenTTD.Domain;
using OpenTTD.Domain.Enums;
using OpenTTD.Domain.Events;
using OpenTTD.Domain.Models;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Actors.Server;

public sealed record NetworkActors(
    IActorRef Sender, 
    IActorRef Receiver);

public enum State
{
    IDLE,
    CONNECTING,
    CONNECTED,
    ERROR
}

public abstract record Model(
    ServerId Id, 
    ServerCredentials Credentials);
public abstract record NetworkModel(
    ServerId Id, 
    ServerCredentials Credentials, 
    NetworkActors Network) : Model(Id, Credentials);

public sealed partial class ServerActor : FSM<State, Model>
{
    private readonly IServerDispatcher _dispatcher;
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    private readonly TcpClient _client = new();

    public ServerActor(ServerId id, ServerCredentials credentials, IServerDispatcher dispatcher, IMediator mediator)
    {
        _dispatcher = dispatcher;

        StartWith(State.IDLE, new Idle(id, credentials));
        
        When(State.IDLE, IdleHandler);
        When(State.CONNECTING, ConnectingHandler);
        When(State.CONNECTED, ConnectedHandler);
        When(State.ERROR, ErrorHandler);
        
        WhenUnhandled(DefaultHandler);
        
        OnTransition((prev, next) =>
        {
            if (prev == next)
            {
                return;
            }
            
            _logger.Debug("[{ServerId}] Changing state from {Prev} to {Next}", id.Value, prev, next);
            
            if (next is State.ERROR or State.IDLE && StateData is NetworkModel model)
            {
                model.Network.Receiver.Tell(PoisonPill.Instance);
                model.Network.Sender.Tell(PoisonPill.Instance);
            }

            if (next is State.ERROR)
            {
                Self.Tell(new ErrorOccurred(), Sender);
            }

            mediator.Publish(new ServerStateChanged(id, MapServerState(prev), MapServerState(next)));
        });
        
        Initialize();
    }
    
    private State<State, Model> DefaultHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        ({ } model, Disconnect) => F.Run(() => 
            GoTo(State.IDLE).Using(new Idle(model.Id, model.Credentials))),

        var ((id, credentials), _) => F.Run(() =>
        {
            Self.Tell(new ErrorOccurred(), Sender);

            return GoTo(State.ERROR).Using(new Error(id, credentials)
            {
                Exception = new InvalidOperationException(),
                Message = $"{nameof(DefaultHandler)} Unhandled message"
            });
        }),
        
        _ => throw new InvalidOperationException()
    };

    private static ServerState MapServerState(State state) => state switch
    {
        State.IDLE => ServerState.IDLE,
        State.CONNECTING => ServerState.CONNECTING,
        State.CONNECTED => ServerState.CONNECTED,
        State.ERROR => ServerState.ERROR,
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };
}