using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secret_Santa_App.DTOs.Reponses;
using Secret_Santa_App.DTOs.Requests;
using Secret_Santa_App.Exceptions;
using Secret_Santa_App.Models;
using Secret_Santa_App.Services.Interfaces;

namespace Secret_Santa_App.Controllers;

/// <summary>
/// The controller for handling User related requests.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class
    /// </summary>
    /// <param name="userService">Injected User Service</param>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }


    /// <summary>
    /// Creates new User. Only Admin can add User.
    /// </summary>
    /// <param name="createUserDto">The request data.</param>
    /// <returns>The HTTP response indicating if this request was successful or not.</returns>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser(CreateUserDto createUserDto)
    {
        try
        {
            var newUser = await _userService.UserRegisterAsync(createUserDto);

            return Ok(newUser);
        }
        catch (InvalidInputDataException)
        {
            return BadRequest("Invalid Input");
        }
        catch (UserAlreadyExistsException)
        {
            return BadRequest("User already exists!");
        }
    }

    /// <summary>
    /// Logs in new User. Every existing User in database can call this endpoint.
    /// </summary>
    /// <param name="loginUserDto">The request data.</param>
    /// <returns>The HTTP response indicating if this request was successful or not.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ReadUserDto>> LoginUser(LoginUserDto loginUserDto)
    {
        try
        {
            var loggedInUser = await _userService.UserLoginAsync(loginUserDto);

            return Ok(loggedInUser);
        }
        catch (InvalidCredentialsException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Gets all existing pairs from the database. Only Admin can call this endpoint.
    /// </summary>
    /// <returns>The HTTP response indicating if this request was successful or not.</returns>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadUserPairsDto>>> GetAllPairs()
    {
        var listOfUsers = await _userService.GetAllUserPairsAsync();

        return Ok(listOfUsers);
    }

    /// <summary>
    /// Gets specifed pair from the database. User can call this method only for himself (thus only with his/her Id)
    /// </summary>
    /// <param name="id">The User Id</param>
    /// <returns>The HTTP response indicating if this request was successful or not.</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadUserPairsDto>> GetUserPair(int id)
    {
        try
        {
            var userPair =
                await _userService.GetUserRecepientAsync(id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            return Ok(userPair);
        }
        catch (UserNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    /// <summary>
    /// Generates pairs for all existing users. Only Admin can generate pairs.
    /// </summary>
    /// <returns>The HTTP response indicating if this request was successful or not.</returns>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("GeneratePairs")]
    public async Task<ActionResult> GeneratePairs()
    {
        try
        {
            await _userService.GenerateUserPairsAsync();

            return Ok();
        }
        catch (InsufficientNumberOfUsersException)
        {
            return BadRequest();
        }
    }
}