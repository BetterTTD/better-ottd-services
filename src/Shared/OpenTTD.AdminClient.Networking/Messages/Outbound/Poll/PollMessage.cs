using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Outbound.Poll;

public sealed record PollMessage(UpdateType UpdateType, uint Argument) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_POLL;
}