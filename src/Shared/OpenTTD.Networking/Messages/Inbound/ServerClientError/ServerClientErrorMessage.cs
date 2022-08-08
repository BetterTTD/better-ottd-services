using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerClientError;

public sealed record ServerClientErrorMessage(
    uint ClientId, 
    NetworkErrorCode Error) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_ERROR;
}