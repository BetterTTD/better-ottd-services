using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerProtocol;

public sealed class AdminServerProtocolTransformer : IMessageTransformer<AdminServerProtocolMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_PROTOCOL;

    public AdminServerProtocolMessage Transform(Packet packet)
    {
        var updateFrequencies = new Dictionary<AdminUpdateType, UpdateFrequency>();
        var version = packet.ReadByte();

        while (packet.ReadBool())
        {
            var updateType = (AdminUpdateType)packet.ReadU16();
            var frequency = packet.ReadU16();
            updateFrequencies.Add(updateType, (UpdateFrequency)frequency);
        }

        var msg = new AdminServerProtocolMessage(version, updateFrequencies);
        return msg;
    }
}