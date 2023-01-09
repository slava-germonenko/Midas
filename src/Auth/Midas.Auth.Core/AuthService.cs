using Microsoft.EntityFrameworkCore;
using Midas.Auth.Core.Constants;
using Midas.Auth.Core.Contracts;
using Midas.Auth.Core.Models;
using Midas.Core;
using Midas.Core.Contracts;
using Midas.Core.Exceptions;
using Midas.Core.Extensions;
using Midas.Core.Models;

namespace Midas.Auth.Core;

public class AuthService
{
    private static readonly TimeSpan SessionRefreshSpan = TimeSpan.FromDays(1);

    private readonly MidasContext _context;

    private readonly IAccessTokenGenerator _accessTokenGenerator;

    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        MidasContext context,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IPasswordHasher passwordHasher
    )
    {
        _context = context;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<(SecurityToken accessToken, SecurityToken refreshToken)> AuthorizeAsync(AuthRequest authRequest)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(authRequest.Login) || u.Phone.Equals(authRequest.Login));

        if (user is null)
        {
            throw new CoreLogicException(Errors.InvalidCredentials);
        }

        if (!_passwordHasher.PasswordIsValid(user, authRequest.Password))
        {
            throw new CoreLogicException(Errors.InvalidCredentials);
        }

        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.UserId == user.Id && s.IpAddress.Equals(authRequest.IpAddress));

        if (session?.ExpireDate <= DateTime.UtcNow)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            session = null;
        }

        if (session is null)
        {
            session = new Session
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
            };
        }

        var refreshToken = _refreshTokenGenerator.Generate(user.Id);
        var accessToken = _accessTokenGenerator.Generate(user.Id);

        session.ExpireDate = DateTime.UtcNow.Add(SessionRefreshSpan);
        session.IpAddress = authRequest.IpAddress;
        session.Name = authRequest.Device;

        _context.Sessions.Update(session);
        await _context.SaveChangesAsync();

        return (accessToken, refreshToken);
    }

    public async Task<(SecurityToken accessToken, SecurityToken refreshToken)> RefreshSessionAsync(string refreshToken)
    {
        var session = await _context.Sessions.FirstOrDefaultAsync(s => s.RefreshToken.Equals(refreshToken));
        if (session is null || session.ExpireDate <= DateTime.UtcNow)
        {
            throw new CoreLogicException("Session has already expired.");
        }

        var newRefreshToken = _refreshTokenGenerator.Generate(session.UserId);
        var newAccessToken = _accessTokenGenerator.Generate(session.UserId);

        session.RefreshToken = newRefreshToken.Token;
        session.ExpireDate = newRefreshToken.ExpireDate;

        return (newAccessToken, newRefreshToken);
    }
}