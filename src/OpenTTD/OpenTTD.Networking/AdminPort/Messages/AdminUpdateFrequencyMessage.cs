using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminUpdateFrequencyMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_ADMIN_UPDATE_FREQUENCY;

    public AdminUpdateType UpdateType { get; init; }
    public UpdateFrequency UpdateFrequency { get; init; }

    public override string ToString() => $"Update FREQ {UpdateType} to {UpdateFrequency}";
}