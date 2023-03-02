using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Events;

public sealed record ServerError(ServerId ServerId, Exception Exception, string Message) : BaseEvent;