namespace Secret_Santa_App.DTOs.Requests;


/// <summary>
/// Data Transfer Object that serves as a request for creating a user.
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// Gets or sets the FirstName.
    /// </summary>
    /// <value>
    /// The FirstName.
    /// </value>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the LastName.
    /// </summary>
    /// <value>
    /// The LastName.
    /// </value>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the Email.
    /// </summary>
    /// <value>
    /// The Email.
    /// </value>
    public string? Email { get; set; }
}