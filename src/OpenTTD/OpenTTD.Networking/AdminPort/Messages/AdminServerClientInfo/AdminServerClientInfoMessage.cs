using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientInfo;

public sealed record AdminServerClientInfoMessage : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_INFO;

    public uint ClientId { get; init; }
    public string Hostname { get; init; }
    public string ClientName { get; init; }
    public byte Language { get; init; }
    public long JoinDate { get; init; }
    public byte PlayingAs { get; init; }
}