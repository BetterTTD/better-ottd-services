namespace OpenTTD.SignalrHub.Messages.Servers;

public record AskServers;

public record AskServer(Guid ServerId);