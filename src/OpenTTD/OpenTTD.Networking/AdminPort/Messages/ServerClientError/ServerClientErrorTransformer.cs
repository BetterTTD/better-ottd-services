using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerClientError;

public sealed class ServerClientErrorTransformer : IPacketTransformer<ServerClientErrorMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_ERROR;

    public ServerClientErrorMessage Transform(Packet packet)
    {
        var msg = new ServerClientErrorMessage(packet.ReadU32(), (NetworkErrorCode) packet.ReadByte());
        return msg;
    }
}