using System.Diagnostics.Contracts;
using OpenTTD.Networking.Enums;
using OpenTTD.Networking.Messages;

namespace OpenTTD.Networking;

public interface IMessageDeserializer
{
    [Pure]
    public IMessage Deserialize(PacketType type, string json);
}