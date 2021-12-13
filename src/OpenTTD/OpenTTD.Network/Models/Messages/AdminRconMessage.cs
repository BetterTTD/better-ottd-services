using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminRconMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminRcon;

    public string Command { get; }

    public AdminRconMessage(string command)
    {
        Command = command;
    }

}