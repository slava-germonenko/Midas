using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Midas.Auth.Core.Contracts;
using Midas.Auth.Core.Models;
using Midas.Auth.Infrastructure.Options;

namespace Midas.Auth.Infrastructure.Contract;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly IOptionsSnapshot<RefreshTokensOptions> _refreshTokenOptions;

    private TimeSpan RefreshTokenLifeSpan =>
        TimeSpan.FromMinutes(_refreshTokenOptions.Value.RefreshTokenLifeSpanMinutes);

    public RefreshTokenGenerator(IOptionsSnapshot<RefreshTokensOptions> refreshTokenOptions)
    {
        _refreshTokenOptions = refreshTokenOptions;
    }

    public SecurityToken Generate(int userId)
    {
        var userIdBytes = BitConverter.GetBytes(userId);
        var randomBytes = RandomNumberGenerator.GetBytes(_refreshTokenOptions.Value.RefreshTokenRandomBytesNumber);
        var allBytesSequence = userIdBytes.Concat(randomBytes).ToArray();
        var now = DateTime.UtcNow;
        return new()
        {
            Token = Convert.ToBase64String(allBytesSequence),
            IssueDate = now,
            ExpireDate = now.Add(RefreshTokenLifeSpan)
        };
    }
}