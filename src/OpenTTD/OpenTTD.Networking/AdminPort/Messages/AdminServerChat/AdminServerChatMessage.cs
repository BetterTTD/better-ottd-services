using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerChat;

public sealed record AdminServerChatMessage : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CHAT;

    public NetworkAction NetworkAction { get; init; }
    public ChatDestination ChatDestination { get; init; }
    public uint ClientId { get; init; }
    public string Message { get; init; }
    public long Data { get; init; }
}