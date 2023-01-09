using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Midas.Api.Models;
using Midas.Api.Models.Auth;
using Midas.Auth.Core;
using Midas.Auth.Core.Models;
using Midas.Auth.Infrastructure.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Midas.Api.Controllers;

[ApiController, Route("auth")]
public class AuthController : ControllerBase
{
    private const string AccessTokenCookieName = "midas-access-token";

    private const string RefreshTokenCookieName = "midas-refresh-token";

    private readonly AuthService _authService;

    private readonly IMapper _mapper;

    private readonly IOptionsSnapshot<AccessTokensOptions> _accessTokensOptions;

    private readonly IOptionsSnapshot<RefreshTokensOptions> _refreshTokenOptions;

    public AuthController(
        AuthService authService,
        IMapper mapper,
        IOptionsSnapshot<AccessTokensOptions> accessTokensOptions,
        IOptionsSnapshot<RefreshTokensOptions> refreshTokenOptions
    )
    {
        _authService = authService;
        _mapper = mapper;
        _accessTokensOptions = accessTokensOptions;
        _refreshTokenOptions = refreshTokenOptions;
    }

    [HttpPost("")]
    [SwaggerOperation("Authorizes user by login and password.")]
    [SwaggerResponse(200, "Access and refresh tokens", typeof(LoginResult), "application/json")]
    [SwaggerResponse(400, "Invalid credentials error", typeof(ErrorResponse), "application/json")]
    public async Task<ActionResult<LoginResult>> AuthorizeAsync(
        [FromBody, SwaggerRequestBody] LoginRequest loginRequest
    )
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (ipAddress is null)
        {
            return BadRequest(new ErrorResponse("Unable to identify IP address of the client."));
        }

        var authRequest = _mapper.Map<AuthRequest>(loginRequest);
        authRequest.IpAddress = ipAddress;
        var (accessToken, refreshToken) = await _authService.AuthorizeAsync(authRequest);
        var result = new LoginResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        SetAccessTokenCookie(accessToken);
        SetRefreshTokenCookie(refreshToken);

        return Ok(result);
    }

    [HttpPost("refresh")]
    [SwaggerOperation(
        "Refreshes user session.",
        "Tries to get refresh token for cookies and refreshes session associated with this token."
    )]
    [SwaggerResponse(200, "Refreshed access and refresh tokens.", typeof(LoginResult), "application/json")]
    [SwaggerResponse(401, "Missing refresh token error.", typeof(ErrorResponse), "application/json")]
    public async Task<ActionResult<LoginResult>> RefreshSessionAsync()
    {
        if (!Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
        {
            return Unauthorized(new ErrorResponse("Refresh token not found."));
        }

        var (accessToken, newRefreshToken) = await _authService.RefreshSessionAsync(refreshToken);
        var result = new LoginResult
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
        };

        return Ok(result);
    }

    private void SetAccessTokenCookie(SecurityToken accessToken)
    {
        var options = new CookieOptions
        {
            Expires = accessToken.ExpireDate,
            SameSite = SameSiteMode.Strict,
            Secure = _accessTokensOptions.Value.UseSecureCookie,
            HttpOnly = false,
        };
        Response.Cookies.Append(AccessTokenCookieName, accessToken.Token, options);
    }

    private void SetRefreshTokenCookie(SecurityToken refreshToken)
    {
        var options = new CookieOptions
        {
            Expires = refreshToken.ExpireDate,
            SameSite = SameSiteMode.Strict,
            Secure = _refreshTokenOptions.Value.UseSecureCookie,
            HttpOnly = true,
        };
        Response.Cookies.Append(RefreshTokenCookieName, refreshToken.Token, options);
    }
}