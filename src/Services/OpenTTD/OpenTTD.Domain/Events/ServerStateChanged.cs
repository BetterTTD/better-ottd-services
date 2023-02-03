using OpenTTD.Domain.Enums;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Events;

public sealed record ServerStateChanged(ServerId ServerId, ServerState FromState, ServerState ToState) : BaseEvent;