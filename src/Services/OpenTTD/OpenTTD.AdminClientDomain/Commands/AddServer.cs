using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Commands;

public sealed record AddServer(ServerNetwork Network) : ICommand<ServerId>;