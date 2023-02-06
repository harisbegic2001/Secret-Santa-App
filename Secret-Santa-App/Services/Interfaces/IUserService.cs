using Secret_Santa_App.DTOs.Reponses;
using Secret_Santa_App.DTOs.Requests;
using Secret_Santa_App.Models;

namespace Secret_Santa_App.Services.Interfaces;

/// <summary>
/// The User service interface.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates new user.
    /// </summary>
    /// <param name="createUserDto">The request body for creating a new user.</param>
    /// <returns>The newly created user.</returns>
    Task<User> UserRegisterAsync(CreateUserDto createUserDto);

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="loginUserDto">The request body for login a user.</param>
    /// <returns>The login response.</returns>
    Task<ReadUserDto> UserLoginAsync(LoginUserDto loginUserDto);

    /// <summary>
    /// Gets all user pairs.
    /// </summary>
    /// <returns>The readonly collection  of users.</returns>
    Task<IEnumerable<ReadUserPairsDto>> GetAllUserPairsAsync();

    /// <summary>
    /// Gets single user pair.
    /// </summary>
    /// <param name="id">The id of user for which the pair will be returned.</param>
    /// <param name="callerId">The id of the caller of the method</param>
    /// <returns></returns>
    Task<ReadUserPairsDto> GetUserRecepientAsync(int id, string callerId);

    /// <summary>
    /// Generates pairs for all existing users.
    /// </summary>
    /// <returns>The task.</returns>
    Task GenerateUserPairsAsync();


}