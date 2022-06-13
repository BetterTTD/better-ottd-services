using Networking.Enums;

namespace Networking.Messages.Inbound.ServerChat;

public sealed record ServerChatMessage : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CHAT;

    public NetworkAction NetworkAction { get; init; }
    public ChatDestination ChatDestination { get; init; }
    public uint ClientId { get; init; }
    public string Message { get; init; }
    public long Data { get; init; }
}