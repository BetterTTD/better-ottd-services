using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerCompanyNew;

public sealed class ServerCompanyNewTransformer : IPacketTransformer<ServerCompanyNewMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_NEW;

    public ServerCompanyNewMessage Transform(Packet packet)
    {
        var msg = new ServerCompanyNewMessage(packet.ReadByte());
        return msg;
    }
}