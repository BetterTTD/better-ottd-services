using OpenTTD.Network.Enums;
using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminServerDateMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerDate;

    public OttdDate Date { get; }

    public AdminServerDateMessage(uint date)
    {
        Date = new OttdDate(date);
    }
}