using Domain.ValueObjects;
using MediatR;

namespace OpenTTD.AdminClient.Domain.Commands.DisconnectServer;

public sealed record DisconnectServerCommand(ServerId ServerId) : IRequest<ServerId>;