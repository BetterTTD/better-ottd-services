using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.Base;

public sealed record GenericAdminMessage : IAdminMessage
{
    public AdminPacketType PacketType { get; init; }
}