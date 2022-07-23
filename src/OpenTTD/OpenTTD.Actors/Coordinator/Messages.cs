using Domain.Models;
using Domain.ValueObjects;

namespace OpenTTD.Actors.Coordinator;

public sealed record ServerAdd(ServerId ServerId, ServerCredentials Credentials);
public sealed record ServerConnect(ServerId ServerId);
public sealed record ServerDisconnect(ServerId ServerId);
public sealed record ServerRemove(ServerId ServerId);

public sealed record ServerAdded(ServerId Id);
