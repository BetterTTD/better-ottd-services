using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminServerRconMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerRcon;

    public ushort Color { get; }

    public string Result { get; }

    public AdminServerRconMessage(ushort color, string result)
    {
        Color = color;
        Result = result;
    }
}