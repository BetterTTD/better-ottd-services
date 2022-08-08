using CSharpFunctionalExtensions;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;

namespace OpenTTD.Domain.Commands.AddServer;

public record AddServerCommand(ServerCredentials Credentials) : IRequest<Result<ServerId>>;