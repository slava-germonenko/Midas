using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Midas.Api.Models;
using Midas.Api.Models.Users;
using Midas.Users.Core.Services;

namespace Midas.Api.Controllers;

[ApiController, Route("users"), Authorize]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;

    public UsersController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet("{userId:int}")]
    [SwaggerOperation("Gets user by id")]
    [SwaggerResponse(200, "Found user model.", typeof(UserViewModel), "application/json")]
    [SwaggerResponse(404, "Not found error.", typeof(ErrorResponse), "application/json")]
    public async Task<ActionResult<UserViewModel>> GetUserAsync(
        [FromRoute, SwaggerParameter("User ID to search.", Required = true)] int userId,
        [FromServices] SearchUsersService usersService
    )
    {
        var user = await usersService.GetUserAsync(userId);
        return Ok(_mapper.Map<UserViewModel>(user));
    }
}