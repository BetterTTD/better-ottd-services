using OpenTTD.Network.Enums;
using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminServerClientInfoMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientInfo;

    public uint ClientId { get; set; }

    public string Hostname { get; set; }

    public string ClientName { get; set; }

    public byte Language { get; set; }

    public OttdDate JoinDate { get; set; }

    public byte PlayingAs { get; set; }
}