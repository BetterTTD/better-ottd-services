using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Commands;

public sealed record ConnectServer(ServerId Id) : ICommand<ServerId>;