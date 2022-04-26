using System.Net.Sockets;
using OpenTTD.Domain;

namespace OpenTTD.Actors.Server;

public sealed record Connecting(ServerCredentials Credentials) : Model;

public sealed partial class ServerActor
{
    private State<State, Model> ConnectingHandler(Event<Model> @event)
    {
        if (@event.StateData is not Connecting connecting)
        {
            return GoTo(State.Error).Using(new Error());
        }

        return Stay();
    }
}