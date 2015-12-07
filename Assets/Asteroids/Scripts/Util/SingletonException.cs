using System;
using System.Runtime.Serialization;

[Serializable]
internal class SingletonException : Exception
{
    public SingletonException()
    {
    }

    public SingletonException(string message) : base(message)
    {
    }

    public SingletonException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected SingletonException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}