using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminPingMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminPing;

    public uint Argument { get; set; }

    public AdminPingMessage(uint argument = 0)
    {
        Argument = argument;
    }
}