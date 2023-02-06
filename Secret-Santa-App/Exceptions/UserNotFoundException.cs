namespace Secret_Santa_App.Exceptions;

/// <summary>
/// User not found exception class.
/// </summary>
public class UserNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
    /// </summary>
    public UserNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public UserNotFoundException(string message) : base(message)
    {
    }
}