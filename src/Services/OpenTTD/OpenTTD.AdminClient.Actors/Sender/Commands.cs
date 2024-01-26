using OpenTTD.AdminClient.Actors.Base;
using OpenTTD.AdminClient.Networking.Messages;

namespace OpenTTD.AdminClient.Actors.Sender;

public sealed record SendMessage(IMessage Message) : ICommand;
