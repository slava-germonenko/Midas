namespace Midas.Core.Exceptions;

public class DuplicateException : CoreLogicException
{
    public DuplicateException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}