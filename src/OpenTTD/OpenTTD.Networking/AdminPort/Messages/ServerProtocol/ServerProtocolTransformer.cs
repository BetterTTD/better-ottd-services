using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerProtocol;

public sealed class ServerProtocolTransformer : IPacketTransformer<ServerProtocolMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PROTOCOL;

    public ServerProtocolMessage Transform(Packet packet)
    {
        var updateFrequencies = new Dictionary<AdminUpdateType, UpdateFrequency>();
        var version = packet.ReadByte();

        while (packet.ReadBool())
        {
            var updateType = (AdminUpdateType)packet.ReadU16();
            var frequency = packet.ReadU16();
            updateFrequencies.Add(updateType, (UpdateFrequency)frequency);
        }

        var msg = new ServerProtocolMessage(version, updateFrequencies);
        return msg;
    }
}