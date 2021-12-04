namespace OpenTTD.API.Network.AdminPort;

public class AdminChatMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketAdminChat;

    public NetworkAction NetworkAction { get; }

    public ChatDestination ChatDestination { get; }

    public uint Destination { get; }

    public string Message { get; }

    public AdminChatMessage(NetworkAction networkAction, ChatDestination chatDestination, uint destination, string message)
    {
        NetworkAction = networkAction;
        ChatDestination = chatDestination;
        Message = message;
        Destination = destination;
    }
}