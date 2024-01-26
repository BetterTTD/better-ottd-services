using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record AddServer(ServerNetwork Network) : ICommand<ServerId>;