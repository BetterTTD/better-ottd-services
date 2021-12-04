namespace OpenTTD.API.Network.AdminPort;

public class AdminServerPongMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerPong;

    public uint Argument { get; }

    public AdminServerPongMessage(uint arg) => Argument = arg;
}