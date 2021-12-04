namespace OpenTTD.API.Network.AdminPort;

public class AdminServerClientQuit : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientQuit;
        
    public uint ClientId { get; }

    public AdminServerClientQuit(uint clientId) => ClientId = clientId;
}