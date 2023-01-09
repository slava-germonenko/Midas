namespace Midas.Infrastructure.Options;

public class PasswordHashingOptions
{
    public int IterationsCount { get; set; }

    public int DefaultSaltLength { get; set; }

    public int PasswordBytesNumber { get; set; }
}