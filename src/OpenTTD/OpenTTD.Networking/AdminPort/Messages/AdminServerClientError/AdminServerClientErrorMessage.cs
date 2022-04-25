using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerClientErrorMessage(
    uint ClientId, 
    NetworkErrorCode Error) : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_ERROR;
}

public sealed class AdminServerClientErrorTransformer : IPacketTransformer<AdminServerClientErrorMessage>
{
    public AdminServerClientErrorMessage Transform(Packet packet)
    {
        var msg = new AdminServerClientErrorMessage(packet.ReadU32(), (NetworkErrorCode) packet.ReadByte());
        return msg;
    }
}