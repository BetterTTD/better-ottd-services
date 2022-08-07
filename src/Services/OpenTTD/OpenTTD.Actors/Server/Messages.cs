using Domain.ValueObjects;

namespace OpenTTD.Actors.Server;

public sealed record Connect;
public sealed record Disconnect;

public sealed record ServerStateChanged(ServerId ServerId, State State);