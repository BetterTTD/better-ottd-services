namespace OpenTTD.Network.Models.Enums;

public class AdminUpdateSetting
{
    public bool Enabled { get; }

    public AdminUpdateType UpdateType { get; }

    public UpdateFrequency UpdateFrequency { get; }

    public AdminUpdateSetting(bool enabled, AdminUpdateType updateType, UpdateFrequency updateFrequency)
    {
        Enabled = enabled;
        UpdateType = updateType;
        UpdateFrequency = UpdateFrequency;
    }

    public override string ToString() => $"{UpdateType} - {UpdateFrequency}";

}