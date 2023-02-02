using MediatR;
using OpenTTD.Domain.ValueObjects;
using OpenTTD.Networking.Messages;

namespace OpenTTD.Domain.Events;

public sealed record NetworkMessageSent(ServerId Id, IMessage Message) : INotification;