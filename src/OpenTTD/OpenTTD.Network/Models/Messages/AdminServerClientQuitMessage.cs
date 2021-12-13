using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminServerClientQuitMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientQuit;
        
    public uint ClientId { get; }

    public AdminServerClientQuitMessage(uint clientId) => ClientId = clientId;
}