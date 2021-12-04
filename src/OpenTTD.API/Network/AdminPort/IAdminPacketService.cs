namespace OpenTTD.API.Network.AdminPort;

public interface IAdminPacketService
{
    Packet CreatePacket(IAdminMessage message);

    IAdminMessage ReadPacket(Packet packet);
}