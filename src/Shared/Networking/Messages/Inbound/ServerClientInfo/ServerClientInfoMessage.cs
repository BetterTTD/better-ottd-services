using Networking.Enums;

namespace Networking.Messages.Inbound.ServerClientInfo;

public sealed record ServerClientInfoMessage : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_INFO;

    public uint ClientId { get; init; }
    public string Hostname { get; init; }
    public string ClientName { get; init; }
    public byte Language { get; init; }
    public long JoinDate { get; init; }
    public byte CompanyId { get; init; }
}