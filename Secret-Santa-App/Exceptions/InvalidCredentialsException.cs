namespace Secret_Santa_App.Exceptions;

/// <summary>
/// Invalid credentials exception class.
/// </summary>
public class InvalidCredentialsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCredentialsException"/> class.
    /// </summary>
    public InvalidCredentialsException()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCredentialsException"/> class.
    /// </summary>
    /// <param name="message">The message to be returned.</param>
    public InvalidCredentialsException(string message) : base(message)
    {
        
    }
}