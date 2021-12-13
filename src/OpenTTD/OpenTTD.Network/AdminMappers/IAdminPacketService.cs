using OpenTTD.Network.Models;
using OpenTTD.Network.Models.Messages;

namespace OpenTTD.Network.AdminMappers;

public interface IAdminPacketService
{
    Packet CreatePacket(IAdminMessage message);

    IAdminMessage ReadPacket(Packet packet);
}