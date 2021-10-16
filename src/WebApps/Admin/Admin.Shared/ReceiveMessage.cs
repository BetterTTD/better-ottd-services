namespace Admin.Shared
{
    public sealed class ReceiveMessage
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