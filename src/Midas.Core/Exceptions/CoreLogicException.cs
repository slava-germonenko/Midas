namespace Midas.Core.Exceptions;

public class CoreLogicException : Exception
{
    public CoreLogicException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}