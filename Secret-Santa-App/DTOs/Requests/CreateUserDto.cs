namespace Secret_Santa_App.DTOs.Requests;

public class CreateUserDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }
}