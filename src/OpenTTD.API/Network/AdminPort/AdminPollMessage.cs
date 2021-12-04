namespace OpenTTD.API.Network.AdminPort;

public class AdminPollMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminPoll;

    public AdminUpdateType UpdateType { get; }

    public uint Argument { get; }

    public AdminPollMessage(AdminUpdateType updateType, uint argument)
    {
        UpdateType = updateType;
        Argument = argument;
    }
}