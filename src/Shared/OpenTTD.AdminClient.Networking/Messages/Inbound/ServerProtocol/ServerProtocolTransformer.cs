using OpenTTD.AdminClient.Networking.Common;
using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound.ServerProtocol;

public sealed class ServerProtocolTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PROTOCOL;

    public IMessage Transform(Packet packet)
    {
        var updateFrequencies = new Dictionary<UpdateType, Enums.UpdateFrequency>();
        var version = packet.ReadByte();

        while (packet.ReadBool())
        {
            var updateType = (UpdateType)packet.ReadU16();
            var frequency = packet.ReadU16();
            updateFrequencies.Add(updateType, (Enums.UpdateFrequency)frequency);
        }

        var msg = new ServerProtocolMessage(version, updateFrequencies);
        return msg;
    }
}