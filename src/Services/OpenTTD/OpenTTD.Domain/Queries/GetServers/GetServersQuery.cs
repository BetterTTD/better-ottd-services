using MediatR;

namespace OpenTTD.Domain.Queries.GetServers;

public record GetServers : IRequest<List<ServerViewModel>>;

public record ServerViewModel;