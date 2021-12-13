using OpenTTD.Network.AdminPort.Messages;
using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort;

public interface IAdminPacketService
{
    Packet CreatePacket(IAdminMessage message);

    IAdminMessage ReadPacket(Packet packet);
}