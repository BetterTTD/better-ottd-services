using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound.UpdateFrequency;

public sealed record UpdateFrequencyMessage(
    AdminUpdateType UpdateType,
    Enums.UpdateFrequency UpdateFrequency) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_UPDATE_FREQUENCY;
}