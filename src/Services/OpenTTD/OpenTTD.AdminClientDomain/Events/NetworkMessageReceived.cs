using OpenTTD.AdminClientDomain.ValueObjects;
using OpenTTD.Networking.Messages;

namespace OpenTTD.AdminClientDomain.Events;

public sealed record NetworkMessageReceived(ServerId ServerId, IMessage Message) : BaseEvent;