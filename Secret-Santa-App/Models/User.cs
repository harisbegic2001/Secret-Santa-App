using System.ComponentModel.DataAnnotations;

namespace Secret_Santa_App.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }
    
    public Role UserRole { get; set; }

    public byte[]? PasswordHash { get; set; }
    
    public byte[]? PasswordSalt { get; set; }

    public int? GiftRecepientId { get; set; }

    public string? RecepientFullName { get; set; }
}