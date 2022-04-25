using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientUpdate;

public sealed record AdminServerClientUpdateMessage: IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_UPDATE;
    
    public uint ClientId { get; init; }
    public string ClientName { get; init; }
    public byte CompanyId { get; init; }
}