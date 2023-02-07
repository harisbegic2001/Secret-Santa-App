namespace Secret_Santa_App.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException()
    {
        
    }

    public UserAlreadyExistsException(string message) : base(message)
    {
        
    }
}