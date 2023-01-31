using OpenTTD.Actors.Base;
using OpenTTD.Networking.Messages;

namespace OpenTTD.Actors.Sender;

public sealed record SendMessage(IMessage Message) : ICommand;
