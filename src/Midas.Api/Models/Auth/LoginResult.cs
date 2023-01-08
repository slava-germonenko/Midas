using Midas.Auth.Core.Models;

namespace Midas.Api.Models.Auth;

public record LoginResult
{
    public required SecurityToken AccessToken { get; set; }

    public required SecurityToken RefreshToken { get; set; }
}