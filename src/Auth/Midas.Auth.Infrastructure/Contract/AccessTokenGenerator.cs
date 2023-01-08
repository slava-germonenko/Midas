using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Midas.Auth.Core.Contracts;
using Midas.Auth.Infrastructure.Options;
using SecurityToken = Midas.Auth.Core.Models.SecurityToken;

namespace Midas.Auth.Infrastructure.Contract;

public class AccessTokenGenerator : IAccessTokenGenerator
{
    private readonly IOptionsSnapshot<AccessTokensOptions> _accessTokenOptions;

    private byte[] SecretKeyBytes => Encoding.UTF8.GetBytes(_accessTokenOptions.Value.JwtSecret);

    private TimeSpan AccessTokenLifeSpan => TimeSpan.FromSeconds(_accessTokenOptions.Value.AccessTokenTtlSeconds);

    public AccessTokenGenerator(IOptionsSnapshot<AccessTokensOptions> accessTokenOptions)
    {
        _accessTokenOptions = accessTokenOptions;
    }

    public SecurityToken Generate(int userId)
    {
        var issuedDate = DateTime.UtcNow;
        var expireDate = issuedDate.Add(AccessTokenLifeSpan);

        var securityKey = new SymmetricSecurityKey(SecretKeyBytes);
        var userIdClaim = new Claim(JwtRegisteredClaimNames.NameId, userId.ToString());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(new []{userIdClaim}),
            Expires = expireDate,
            SigningCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenStringRepresentation = tokenHandler.CreateToken(tokenDescriptor);

        return new()
        {
            IssueDate = issuedDate,
            ExpireDate = expireDate,
            Token = tokenHandler.WriteToken(tokenStringRepresentation),
        };
    }
}