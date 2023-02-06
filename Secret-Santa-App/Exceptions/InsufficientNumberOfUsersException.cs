namespace Secret_Santa_App.Exceptions;

/// <summary>
/// Insufficient number of users exception class.
/// </summary>
public class InsufficientNumberOfUsersException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InsufficientNumberOfUsersException"/> class.
    /// </summary>
    public InsufficientNumberOfUsersException()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InsufficientNumberOfUsersException"/> class.
    /// </summary>
    /// <param name="message">The message to be returned.</param>
    public InsufficientNumberOfUsersException(string message) : base(message)
    {
        
    }
}