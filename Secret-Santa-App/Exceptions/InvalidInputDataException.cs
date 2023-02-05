namespace Secret_Santa_App.Exceptions;

public class InvalidInputDataException : Exception
{


    public InvalidInputDataException()
    {
        
    }
    public InvalidInputDataException(string message) : base(message)
    {
        
    }
}