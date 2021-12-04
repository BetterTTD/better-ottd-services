namespace OpenTTD.API.Network.AdminPort;

public class AdminServerChatMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerChat;

    public NetworkAction NetworkAction { get; internal set; }

    public ChatDestination ChatDestination { get; internal set; }

    public uint ClientId { get; internal set; }

    public string Message { get; internal set; }

    public long Data { get; internal set; }
    
}