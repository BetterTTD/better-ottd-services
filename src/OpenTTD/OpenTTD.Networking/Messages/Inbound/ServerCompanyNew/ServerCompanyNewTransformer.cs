using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerCompanyNew;

public sealed class ServerCompanyNewTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_NEW;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerCompanyNewMessage(packet.ReadByte());
        return msg;
    }
}