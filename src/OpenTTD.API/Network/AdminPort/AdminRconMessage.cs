namespace OpenTTD.API.Network.AdminPort;

public class AdminRconMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminRcon;

    public string Command { get; }

    public AdminRconMessage(string command)
    {
        Command = command;
    }

}