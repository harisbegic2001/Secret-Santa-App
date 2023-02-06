namespace Secret_Santa_App.EnvironmentSettings;

/// <summary>
/// Secret configuration for IOptions.
/// </summary>
public class SecretConfiguration
{
    /// <summary>
    /// Gets or sets the Secret Key.
    /// </summary>
    /// <value>
    /// The Secret Key for symmetric security key.
    /// </value>
    public string? SecretKey { get; set; }
}