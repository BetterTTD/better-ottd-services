using OpenTTD.AdminClientDomain.Enums;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Events;

public sealed record ServerStateChanged(ServerId ServerId, ServerState FromState, ServerState ToState) : BaseEvent;