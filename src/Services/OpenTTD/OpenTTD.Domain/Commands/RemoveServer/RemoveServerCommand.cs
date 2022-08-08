using CSharpFunctionalExtensions;
using Domain.ValueObjects;
using MediatR;

namespace OpenTTD.Domain.Commands.RemoveServer;

public record RemoveServerCommand(ServerId ServerId) : IRequest<Result<ServerId>>;