using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.Base;

public interface IMessageTransformer<out TMessage> where TMessage : IAdminMessage
{
    AdminPacketType PacketType { get; }
    TMessage Transform(Packet packet);
}