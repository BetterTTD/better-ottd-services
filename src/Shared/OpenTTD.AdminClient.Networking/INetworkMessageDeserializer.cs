using System.Diagnostics.Contracts;
using OpenTTD.AdminClient.Networking.Enums;
using OpenTTD.AdminClient.Networking.Messages;

namespace OpenTTD.AdminClient.Networking;

public interface INetworkMessageDeserializer
{
    [Pure]
    public IMessage Deserialize(PacketType type, string json);
}