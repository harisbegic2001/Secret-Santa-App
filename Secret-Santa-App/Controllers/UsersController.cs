using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secret_Santa_App.DTOs.Reponses;
using Secret_Santa_App.DTOs.Requests;
using Secret_Santa_App.Models;
using Secret_Santa_App.Services.Interfaces;

namespace Secret_Santa_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }


    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser(CreateUserDto createUserDto)
    {
        var newUser = await _userService.UserRegisterAsync(createUserDto);

        return Ok(newUser);
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ReadUserDto>> LoginUser(LoginUserDto loginUserDto)
    {
        var loggedInUser = await _userService.UserLoginAsync(loginUserDto);

        return Ok(loggedInUser);
    }

   // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadUserPairsDto>>> GetAllPairs()
    {
        var listOfUsers = await _userService.GetAllUserPairsAsync();

        return Ok(listOfUsers);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadUserPairsDto>> GetUserPair(int id)
    {
        var userPair = await _userService.GetUserRecepientAsync(id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        
        return Ok(userPair);
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("GeneratePairs")]
    public async Task<ActionResult> GeneratePairs()
    {
        await _userService.GenerateUserPairsAsync();

        return Ok();
    }
}