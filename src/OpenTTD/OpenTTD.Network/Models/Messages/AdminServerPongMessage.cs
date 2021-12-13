using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminServerPongMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerPong;

    public uint Argument { get; }

    public AdminServerPongMessage(uint arg) => Argument = arg;
}