using OpenTTD.AdminClient.Actors.Base;

namespace OpenTTD.AdminClient.Actors.Server;

public sealed record Connect : ICommand;
public sealed record Disconnect : ICommand;
public sealed record Reconnect : ICommand;
