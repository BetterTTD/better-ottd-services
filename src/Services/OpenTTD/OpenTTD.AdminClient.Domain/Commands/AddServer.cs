using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record AddServer(ServerId id, ServerNetwork Network) : ICommand<ServerId>;