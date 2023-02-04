namespace Secret_Santa_App.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
    {
        
    }

    public InvalidCredentialsException(string message) : base(message)
    {
        
    }
}