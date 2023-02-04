namespace Secret_Santa_App.DTOs.Reponses;

public class ReadUserDto
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
}