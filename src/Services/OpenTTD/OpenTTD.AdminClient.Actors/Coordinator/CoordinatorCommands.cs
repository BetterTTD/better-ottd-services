using OpenTTD.AdminClient.Actors.Base;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Actors.Coordinator;

public sealed record ServerAdd(ServerId Id, ServerNetwork Network) : ICommand;
public sealed record ServerConnect(ServerId ServerId) : ICommand;
public sealed record ServerDisconnect(ServerId ServerId) : ICommand;
public sealed record ServerRemove(ServerId ServerId) : ICommand;
