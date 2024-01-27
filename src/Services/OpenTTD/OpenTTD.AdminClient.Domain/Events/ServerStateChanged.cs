using MediatR;
using OpenTTD.AdminClient.Domain.Enums;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Events;

public sealed record ServerStateChanged(ServerId ServerId, ServerState FromState, ServerState ToState) : INotification;