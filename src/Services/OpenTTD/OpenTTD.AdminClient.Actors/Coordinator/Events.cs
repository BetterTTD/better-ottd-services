using OpenTTD.AdminClient.Actors.Base;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Actors.Coordinator;

public sealed record ServerAdded(ServerId Id) : IEvent;
