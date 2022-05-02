using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound.Poll;

public sealed record PollMessage(UpdateType UpdateType, uint Argument) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_POLL;
}