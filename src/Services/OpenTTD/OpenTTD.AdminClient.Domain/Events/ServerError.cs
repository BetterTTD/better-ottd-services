using MediatR;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Events;

public sealed record ServerError(ServerId ServerId, Exception Exception, string Message) : INotification;