using Akka.Util;
using OpenTTD.AdminClient.Networking.Messages;

namespace OpenTTD.AdminClient.Actors.Receiver;

public sealed record ReceivedMsg(Result<IMessage> MsgResult);
