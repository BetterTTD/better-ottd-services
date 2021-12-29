using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerPongMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerPong;

    public uint Argument { get; }

    public AdminServerPongMessage(uint arg) => Argument = arg;
}