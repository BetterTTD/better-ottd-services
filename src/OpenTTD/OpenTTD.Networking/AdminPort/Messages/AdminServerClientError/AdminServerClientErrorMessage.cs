using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientError;

public sealed record AdminServerClientErrorMessage(
    uint ClientId, 
    NetworkErrorCode Error) : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_ERROR;
}