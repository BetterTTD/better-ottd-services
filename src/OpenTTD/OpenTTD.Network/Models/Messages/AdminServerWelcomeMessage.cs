using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerWelcomeMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerWelcome;

    public string ServerName { get; set; }

    public string NetworkRevision { get; set; }

    public bool IsDedicated { get; set; }

    public string MapName { get; set; }

    public uint MapSeed { get; set; }

    public Landscape Landscape { get; set; }

    public OttdDate CurrentDate { get; set; }

    public ushort MapWidth { get; set; }

    public ushort MapHeight { get; set; }
}