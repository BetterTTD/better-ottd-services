using OpenTTD.Actors.Base;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Actors.Coordinator;

public sealed record ServerAdded(ServerId Id) : IEvent;
