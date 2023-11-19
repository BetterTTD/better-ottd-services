namespace OpenTTD.AdminClientDomain.ValueObjects;

public sealed record ServerId(Guid Value);

public sealed record ServerName(string Value);

public sealed record ServerPassword(string Value);

public sealed record ServerPort(int Value);

public sealed record ServerVersion(string Value);