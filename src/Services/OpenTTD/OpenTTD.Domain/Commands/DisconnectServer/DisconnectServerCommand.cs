using Domain.ValueObjects;
using MediatR;

namespace OpenTTD.Domain.Commands.DisconnectServer;

public sealed record DisconnectServerCommand(ServerId ServerId) : IRequest<ServerId>;