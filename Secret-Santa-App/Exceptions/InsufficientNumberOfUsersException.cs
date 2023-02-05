namespace Secret_Santa_App.Exceptions;

public class InsufficientNumberOfUsersException : Exception
{
    public InsufficientNumberOfUsersException()
    {
        
    }

    public InsufficientNumberOfUsersException(string message) : base(message)
    {
        
    }
}