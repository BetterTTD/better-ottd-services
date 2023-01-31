using OpenTTD.Actors.Base;

namespace OpenTTD.Actors.Server;

public sealed record Connect : ICommand;
public sealed record Disconnect : ICommand;