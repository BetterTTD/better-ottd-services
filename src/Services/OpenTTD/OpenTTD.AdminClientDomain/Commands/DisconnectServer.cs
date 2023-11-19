using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Commands;

public sealed record DisconnectServer(ServerId Id) : ICommand<ServerId>;