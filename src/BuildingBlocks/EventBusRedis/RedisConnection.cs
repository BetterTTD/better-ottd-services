using StackExchange.Redis;

namespace EventBusRedis;

public sealed class RedisConnection
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisConnection(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public ISubscriber ProducerBuilder()
    {
        return _connectionMultiplexer.GetSubscriber();
    }

    public ISubscriber ConsumerBuilder()
    {
        return _connectionMultiplexer.GetSubscriber();
    }
}