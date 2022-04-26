using System.Net.Sockets;
using OpenTTD.Domain;

namespace OpenTTD.Actors.Server;

public sealed record Idle(ServerCredentials Credentials) : Model;

public sealed partial class ServerActor
{
    private State<State, Model> IdleHandler(Event<Model> @event)
    {
        if (@event.StateData is not Idle((var serverAddress, _) serverCredentials))
        {
            return GoTo(State.Error).Using(new Error());
        }

        if (@event.FsmEvent is Connect)
        {
            try
            {
                var client = new TcpClient();
                client.Connect(serverAddress.IpAddress, serverAddress.Port);
                return GoTo(State.Connecting).Using(new Connecting(serverCredentials));
            }
            catch (Exception exn)
            {
                return GoTo(State.Error).Using(new Error{ Exception = exn, Message = exn.Message});
            }
        }

        return null!;
    }
}