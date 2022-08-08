using Domain.ValueObjects;
using MediatR;

namespace OpenTTD.AdminClient.Domain.Commands.ConnectServer;

public sealed record ConnectServerCommand(ServerId ServerId) : IRequest<ServerId>;