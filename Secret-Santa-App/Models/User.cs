using System.ComponentModel.DataAnnotations;

namespace Secret_Santa_App.Models;

/// <summary>
/// User Model
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the Id.
    /// </summary>
    /// <value>
    /// The Id.
    /// </value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the FirstName.
    /// </summary>
    /// <value>
    /// The FirstName.
    /// </value>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the LastName.
    /// </summary>
    /// <value>
    /// The LastName.
    /// </value>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the Email.
    /// </summary>
    /// <value>
    /// The Email.
    /// </value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the UserRole.
    /// </summary>
    /// <value>
    /// The UserRole.
    /// </value>
    public Role UserRole { get; set; }

    /// <summary>
    /// Gets or sets the PasswordHash.
    /// </summary>
    /// <value>
    /// The PasswordHash.
    /// </value>
    public byte[]? PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the PasswordSalt.
    /// </summary>
    /// <value>
    /// The PasswordSalt.
    /// </value>
    public byte[]? PasswordSalt { get; set; }

    /// <summary>
    /// Gets or sets the GiftRecepientId.
    /// </summary>
    /// <value>
    /// The GiftRecepientId.
    /// </value>
    public int? GiftRecepientId { get; set; }

    /// <summary>
    /// Gets or sets the RecepientFullName.
    /// </summary>
    /// <value>
    /// The RecepientFullName.
    /// </value>
    public string? RecepientFullName { get; set; }
}