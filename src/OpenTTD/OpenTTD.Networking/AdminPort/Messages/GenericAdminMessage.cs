using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record GenericAdminMessage : IAdminMessage
{
    public AdminMessageType MessageType { get; init; }
}