using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record GenericAdminMessage : IAdminMessage
{
    public AdminPacketType PacketType { get; init; }
}