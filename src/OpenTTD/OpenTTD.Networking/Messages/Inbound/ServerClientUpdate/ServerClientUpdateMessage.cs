using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerClientUpdate;

public sealed record ServerClientUpdateMessage: IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_UPDATE;
    
    public uint ClientId { get; init; }
    public string ClientName { get; init; }
    public byte CompanyId { get; init; }
}