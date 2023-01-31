using Akka.Util;
using OpenTTD.Networking.Messages;

namespace OpenTTD.Actors.Receiver;

public sealed record ReceivedMsg(Result<IMessage> MsgResult);
