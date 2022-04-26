using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerClientError;

public sealed record ServerClientErrorMessage(
    uint ClientId, 
    NetworkErrorCode Error) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_ERROR;
}