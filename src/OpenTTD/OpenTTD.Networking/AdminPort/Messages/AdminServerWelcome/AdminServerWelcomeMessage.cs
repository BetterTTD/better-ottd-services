using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerWelcome;

public sealed record AdminServerWelcomeMessage : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_WELCOME;

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