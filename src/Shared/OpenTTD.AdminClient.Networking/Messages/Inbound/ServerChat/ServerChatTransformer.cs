using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound.ServerChat;

public sealed class ServerChatTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CHAT;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerChatMessage
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