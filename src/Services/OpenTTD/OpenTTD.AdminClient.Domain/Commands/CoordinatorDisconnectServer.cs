using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record CoordinatorDisconnectServer(ServerId Id) : ICommand<ServerId>;