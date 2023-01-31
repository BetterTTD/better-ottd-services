using Domain.ValueObjects;
using OpenTTD.Actors.Base;

namespace OpenTTD.Actors.Server;

public sealed record ServerStateChanged(ServerId ServerId, State State) : IEvent;