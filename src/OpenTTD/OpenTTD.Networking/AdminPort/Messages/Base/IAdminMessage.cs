using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.Base;

public interface IAdminMessage
{
    AdminPacketType PacketType { get; }
}