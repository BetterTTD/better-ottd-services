using MediatR;
using OpenTTD.AdminClient.Domain.ValueObjects;
using OpenTTD.AdminClient.Networking.Messages;

namespace OpenTTD.AdminClient.Domain.Events;

public sealed record NetworkMessageSent(ServerId ServerId, IMessage Message) : INotification;