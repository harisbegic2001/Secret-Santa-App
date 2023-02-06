namespace Secret_Santa_App.DTOs.Reponses;

/// <summary>
/// Data Transfer Object that serves as a response for reading user pairs.
/// </summary>
public class ReadUserPairsDto
{
    /// <summary>
    /// Gets or sets the FullName.
    /// </summary>
    /// <value>
    /// The FullName.
    /// </value>
    public string? FullName { get; set; }

    /// <summary>
    /// Gets or sets the RecepientFullName.
    /// </summary>
    /// <value>
    /// The RecepientFullName.
    /// </value>
    public string? RecepientFullName { get; set; }
}