using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerChat;

public sealed class AdminServerChatTransformer : IMessageTransformer<AdminServerChatMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CHAT;

    public AdminServerChatMessage Transform(Packet packet)
    {
        var msg = new AdminServerChatMessage
        {
            NetworkAction = (NetworkAction)packet.ReadByte(),
            ChatDestination = (ChatDestination)packet.ReadByte(),
            ClientId = packet.ReadU32(),
            Message = packet.ReadString(),
            Data = packet.ReadI64()
        };

        return msg;
    }
}