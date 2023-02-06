using Secret_Santa_App.Models;

namespace Secret_Santa_App.Services.Interfaces;

/// <summary>
/// The Token service interface.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates the user token.
    /// </summary>
    /// <param name="user"> the user for which the token will be generated.</param>
    /// <returns>The string value of generated token.</returns>
    string CreateToken(User user);
}