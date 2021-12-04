namespace OpenTTD.API.Network.AdminPort;

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