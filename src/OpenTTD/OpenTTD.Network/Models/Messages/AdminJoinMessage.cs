using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminJoinMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminJoin;

    public string Password { get; }

    public string AdminName { get; }

    public string AdminVersion { get; }

    public AdminJoinMessage(string password, string adminName, string adminVersion)
    {
        Password = password;
        AdminName = adminName;
        AdminVersion = adminVersion;
    }
}