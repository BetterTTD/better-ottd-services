using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Events;

public sealed record ServerError(ServerId ServerId, Exception Exception, string Message) : BaseEvent;