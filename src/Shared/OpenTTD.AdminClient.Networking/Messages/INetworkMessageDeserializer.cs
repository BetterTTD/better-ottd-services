using System.Diagnostics.Contracts;
using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages;

public interface INetworkMessageDeserializer
{
    [Pure]
    public IMessage Deserialize(PacketType type, string json);
}