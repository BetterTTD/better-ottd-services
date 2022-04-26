using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerChat;

public sealed class ServerChatTransformer : IPacketTransformer<ServerChatMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CHAT;

    public ServerChatMessage Transform(Packet packet)
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