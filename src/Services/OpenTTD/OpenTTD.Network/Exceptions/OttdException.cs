using System.Runtime.Serialization;

namespace OpenTTD.Network.Exceptions;

public class OttdException : Exception
{
    public OttdException()
    {
    }

    public OttdException(string message) : base(message)
    {
    }

    public OttdException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected OttdException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}