namespace Midas.Auth.Infrastructure.Options;

public class RefreshTokensOptions
{
    public int RefreshTokenRandomBytesNumber { get; set; }

    public int RefreshTokenLifeSpanMinutes { get; set; }

    public bool UseSecureCookie { get; set; }
}