namespace Secret_Santa_App.Exceptions;

/// <summary>
/// Invalid input data exception class.
/// </summary>
public class InvalidInputDataException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidInputDataException"/> class.
    /// </summary>
    public InvalidInputDataException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidInputDataException"/> class.
    /// </summary>
    /// <param name="message">The message to be returned.</param>
    public InvalidInputDataException(string message) : base(message)
    {
    }
}