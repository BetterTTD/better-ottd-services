using Common;
using OpenTTD.Actors.Receiver;
using OpenTTD.Domain;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Connected(
        ServerCredentials Credentials, 
        NetworkActors Network, 
        Domain.Server Server) : NetworkModel(Network);

    private State<State, Model> ConnectedHandler(Event<Model> @event) => (@event.FsmEvent, @event.StateData) switch
    {
        (ReceivedMsg msg, Connected model) => F.Run(() =>
        {
            _logger.Info(msg.Message.PacketType.ToString());
            return Stay();
        }),
        
        _ => F.Run(() =>
        {
            Self.Tell(new ErrorOccurred(), Sender);

            return GoTo(State.Error).Using(new Error
            {
                Exception = new InvalidOperationException(),
                Message = "Invalid state data"
            });
        })
    };
}