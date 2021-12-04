namespace OpenTTD.API.Network.AdminPort;

public class AdminRconEvent : IAdminEvent
{
    public AdminEventType EventType => AdminEventType.AdminRcon;

    public ServerInfo Server { get; } 

    public string Message { get; }

    public AdminRconEvent(ServerInfo server, string msg)
    {
        Server = server;
        Message = msg;
    }
}