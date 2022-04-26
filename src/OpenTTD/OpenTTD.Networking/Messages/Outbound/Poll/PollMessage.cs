using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound.Poll;

public sealed record PollMessage : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_POLL;

    public AdminUpdateType UpdateType { get; init; }
    public uint Argument { get; init; }
}