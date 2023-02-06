namespace Secret_Santa_App.DTOs.Reponses;

/// <summary>
/// Data Transfer Object that serves as a response after user successful login.
/// </summary>
public class ReadUserDto
{
    /// <summary>
    /// Gets or sets the Id.
    /// </summary>
    /// <value>
    /// The Id.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Email.
    /// </summary>
    /// <value>
    /// The Email.
    /// </value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the Json Web Token.
    /// </summary>
    /// <value>
    /// The JWT.
    /// </value>
    public string? Token { get; set; }
}