namespace Midas.Auth.Infrastructure.Options;

public class AccessTokensOptions
{
    public string JwtSecret { get; set; } = string.Empty;

    public int AccessTokenTtlSeconds { get; set; }

    public bool UseSecureCookie { get; set; }
}