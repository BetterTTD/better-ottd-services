using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminPingMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminPing;

    public uint Argument { get; set; }

    public AdminPingMessage(uint argument = 0)
    {
        Argument = argument;
    }
}