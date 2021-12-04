namespace OpenTTD.API.Network.AdminPort;

public class AdminUpdateFrequencyMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminUpdateFrequency;

    public AdminUpdateType UpdateType { get; }

    public UpdateFrequency UpdateFrequency { get; }

    public AdminUpdateFrequencyMessage(AdminUpdateType updateType, UpdateFrequency updateFrequency)
    {
        UpdateType = updateType;
        UpdateFrequency = updateFrequency;
    }

    public override string ToString() => $"Update FREQ {UpdateType} to {UpdateFrequency}";
}