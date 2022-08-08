using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerWelcome;

public sealed record ServerWelcomeMessage : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_WELCOME;

    public string ServerName { get; init; }
    public string NetworkRevision { get; init; }
    public bool IsDedicated { get; init; }
    public string MapName { get; init; }
    public uint MapSeed { get; init; }
    public Landscape Landscape { get; init; }
    public long CurrentDate { get; init; }
    public ushort MapWidth { get; init; }
    public ushort MapHeight { get; init; }
}