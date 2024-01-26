using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record ConnectServer(ServerId Id) : ICommand<ServerId>;