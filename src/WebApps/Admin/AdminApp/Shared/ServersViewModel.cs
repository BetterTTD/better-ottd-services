using Networking.Enums;

namespace AdminApp.Shared;
                                  
public sealed record ServerCardViewModel
{
    public string Name { get; init; }
    public Landscape Landscape { get; init; }
    public int Clients { get; init; }
    public int Companies { get; init; }
}

public sealed record ServersViewModel
{
    public List<ServerCardViewModel> Servers { get; init; }
}