using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerClientQuitMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientQuit;
        
    public uint ClientId { get; }

    public AdminServerClientQuitMessage(uint clientId) => ClientId = clientId;
}