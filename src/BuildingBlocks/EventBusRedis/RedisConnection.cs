using StackExchange.Redis;

namespace EventBusRedis;

public sealed class RedisConnection(IConnectionMultiplexer connectionMultiplexer)
{
    public ISubscriber ProducerBuilder()
    {
        return connectionMultiplexer.GetSubscriber();
    }

    public ISubscriber ConsumerBuilder()
    {
        return connectionMultiplexer.GetSubscriber();
    }
}