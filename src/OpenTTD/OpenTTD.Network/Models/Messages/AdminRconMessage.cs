using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminRconMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminRcon;

    public string Command { get; }

    public AdminRconMessage(string command)
    {
        Command = command;
    }

}