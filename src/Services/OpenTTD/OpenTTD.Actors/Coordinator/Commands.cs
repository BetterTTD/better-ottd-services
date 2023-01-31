using Domain.Models;
using Domain.ValueObjects;
using OpenTTD.Actors.Base;

namespace OpenTTD.Actors.Coordinator;

public sealed record ServerAdd(ServerCredentials Credentials) : ICommand;
public sealed record ServerConnect(ServerId ServerId) : ICommand;
public sealed record ServerDisconnect(ServerId ServerId) : ICommand;
public sealed record ServerRemove(ServerId ServerId) : ICommand;
