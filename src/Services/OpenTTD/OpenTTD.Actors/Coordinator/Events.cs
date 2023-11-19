using OpenTTD.Actors.Base;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.Actors.Coordinator;

public sealed record ServerAdded(ServerId Id) : IEvent;
