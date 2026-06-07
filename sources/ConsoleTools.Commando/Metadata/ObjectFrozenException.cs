using System.Runtime.Serialization;

namespace DustInTheWind.ConsoleTools.Commando.Metadata;

[Serializable]
public class ObjectFrozenException : Exception
{
    private const string DefaultMessage = "The object was frozen. Any additional changes are not allowed.";

    public ObjectFrozenException()
        : base(DefaultMessage)
    {
    }

    protected ObjectFrozenException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}