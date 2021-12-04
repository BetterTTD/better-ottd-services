namespace OpenTTD.API.Network.AdminPort;

public class AdminServerProtocolMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerProtocol;

    public byte NetworkVersion { get; }

    public Dictionary<AdminUpdateType, UpdateFrequency> AdminUpdateSettings;
    public AdminServerProtocolMessage(byte networkVersion, Dictionary<AdminUpdateType, UpdateFrequency> adminUpdateSettings)
    {
        NetworkVersion = networkVersion;
        AdminUpdateSettings = adminUpdateSettings;
    }
}