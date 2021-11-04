using EventBus.Events;

namespace Admin.Shared
{
    public record ReceiveMessage : IntegrationEvent
    {
        public ReceiveMessage(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public string Name { get; }
        public string Message { get; }
    }
}