using System.Runtime.Serialization;

namespace OpenTTD.API.Network;

public class OttdConnectionException : OttdException
{
    public OttdConnectionException()
    {
    }

    public OttdConnectionException(string message) : base(message)
    {
    }

    public OttdConnectionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected OttdConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}