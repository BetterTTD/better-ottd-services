using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientError;

public sealed class AdminServerClientErrorTransformer : IMessageTransformer<AdminServerClientErrorMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_ERROR;

    public AdminServerClientErrorMessage Transform(Packet packet)
    {
        var msg = new AdminServerClientErrorMessage(packet.ReadU32(), (NetworkErrorCode) packet.ReadByte());
        return msg;
    }
}