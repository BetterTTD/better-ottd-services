using System;
using Polly.Retry;
using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    // ReSharper disable once InconsistentNaming
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
    
}