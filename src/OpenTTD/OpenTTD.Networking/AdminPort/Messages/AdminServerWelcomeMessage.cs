using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerWelcomeMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_WELCOME;

    public string ServerName { get; set; }
    public string NetworkRevision { get; set; }
    public bool IsDedicated { get; set; }
    public string MapName { get; set; }
    public uint MapSeed { get; set; }
    public Landscape Landscape { get; set; }
    public long CurrentDate { get; set; }
    public ushort MapWidth { get; set; }
    public ushort MapHeight { get; set; }
}