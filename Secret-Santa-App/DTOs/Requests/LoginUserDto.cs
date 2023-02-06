namespace Secret_Santa_App.DTOs.Requests;

/// <summary>
/// Data Transfer Object that serves as a request for login.
/// </summary>
public class LoginUserDto
{
    /// <summary>
    /// Gets or sets the Email.
    /// </summary>
    /// <value>
    /// The Email.
    /// </value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the Password.
    /// </summary>
    /// <value>
    /// The Password.
    /// </value>
    public string? Password { get; set; }
}