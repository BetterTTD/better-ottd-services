using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Commands;

public sealed record RemoveServer(ServerId Id) : ICommand<ServerId>;