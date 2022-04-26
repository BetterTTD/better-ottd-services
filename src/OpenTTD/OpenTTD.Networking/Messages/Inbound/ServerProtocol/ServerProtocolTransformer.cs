using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerProtocol;

public sealed class ServerProtocolTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PROTOCOL;

    public IMessage Transform(Packet packet)
    {
        var updateFrequencies = new Dictionary<AdminUpdateType, Enums.UpdateFrequency>();
        var version = packet.ReadByte();

        while (packet.ReadBool())
        {
            var updateType = (AdminUpdateType)packet.ReadU16();
            var frequency = packet.ReadU16();
            updateFrequencies.Add(updateType, (Enums.UpdateFrequency)frequency);
        }

        var msg = new ServerProtocolMessage(version, updateFrequencies);
        return msg;
    }
}