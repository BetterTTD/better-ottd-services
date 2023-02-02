using MediatR;
using OpenTTD.Domain.Enums;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Events;

public sealed record ServerStateChanged(ServerId Id, ServerState FromState, ServerState ToState) : INotification;