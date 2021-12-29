using System.Runtime.Serialization;

namespace OpenTTD.Network.Extensions;

public static class Enums
{
    public static T[] ToArray<T>()
        where T : Enum => Enum.GetValues(typeof(T)).Cast<T>().ToArray();
}

public class TaskWaitException : Exception
{
    public TaskWaitException()
    {
    }

    public TaskWaitException(string message) : base(message)
    {
    }

    public TaskWaitException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TaskWaitException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}