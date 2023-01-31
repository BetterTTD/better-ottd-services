using Domain.ValueObjects;
using OpenTTD.Actors.Base;

namespace OpenTTD.Actors.Coordinator;

public sealed record ServerAdded(ServerId Id) : IEvent;
