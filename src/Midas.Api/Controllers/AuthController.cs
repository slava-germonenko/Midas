using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Midas.Api.Models;
using Midas.Api.Models.Auth;
using Midas.Auth.Core;
using Midas.Auth.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Midas.Api.Controllers;

[ApiController, Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    private readonly IMapper _mapper;

    public AuthController(AuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
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

        return Ok(result);
    }
}