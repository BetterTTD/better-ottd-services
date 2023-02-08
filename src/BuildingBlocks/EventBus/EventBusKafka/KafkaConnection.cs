using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace EventBusKafka;

public sealed class KafkaConnection
{
    private readonly ProducerConfig _producerConfiguration;
    private readonly SchemaRegistryConfig _schemaRegistryConfiguration;
    private readonly ConsumerConfig _consumerConfiguration;
    private object? _producerBuilder;

    public KafkaConnection(
        ProducerConfig producerConfig, 
        ConsumerConfig consumerConfig, 
        SchemaRegistryConfig schemaRegistryConfig)
    {
        _producerConfiguration = producerConfig;
        _consumerConfiguration = consumerConfig;
        _schemaRegistryConfiguration = schemaRegistryConfig;
    }

    public IProducer<Null, T> ProducerBuilder<T>()
    {
        if (_producerBuilder != null)
        {
            return (IProducer<Null, T>)_producerBuilder;
        }

        var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfiguration);
        _producerBuilder = new ProducerBuilder<Null, T>(_producerConfiguration)
            .SetValueSerializer(new AvroSerializer<T>(schemaRegistry))
            .Build();

        return (IProducer<Null, T>)_producerBuilder;
    }

    public IConsumer<Null, T> ConsumerBuilder<T>()
    {
        var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfiguration);
        var consumer = new ConsumerBuilder<Null, T>(_consumerConfiguration)
            .SetValueDeserializer(new AvroDeserializer<T>(schemaRegistry).AsSyncOverAsync())
            .Build();
        
        return consumer;
    }
}