using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus;

public class EventBusSubscriptionManager : IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<Type>> _eventSubscriptions = new();

    public void Clear() => _eventSubscriptions.Clear();

    public bool IsEmpty => !_eventSubscriptions.Any();

    public void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
           
        if (!HasEvent<T>())
        {
            _eventSubscriptions.Add(eventName, new List<Type>());
        }

        if (_eventSubscriptions[eventName].Any(si => si == typeof(TH)))
        {
            throw new ArgumentException($"HandlerType {typeof(TH).Name} is already registered");
        }

        _eventSubscriptions[eventName].Add(typeof(TH));

    }

    public IEnumerable<Type> GetHandlersForEvent<T>() where T : IntegrationEvent => _eventSubscriptions[GetEventKey<T>()];

    public bool HasEvent<T>() where T : IntegrationEvent => _eventSubscriptions.ContainsKey(GetEventKey<T>());

    public void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        var type = _eventSubscriptions[eventName].SingleOrDefault(si => si == typeof(TH));
        if (type is not null)
        {
            _eventSubscriptions[eventName].Remove(type);
        }
    }

    private static string GetEventKey<T>() => typeof(T).Name;
}