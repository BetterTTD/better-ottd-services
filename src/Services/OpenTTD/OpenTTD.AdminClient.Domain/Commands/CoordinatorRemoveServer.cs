using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record CoordinatorRemoveServer(ServerId Id) : ICommand<ServerId>;