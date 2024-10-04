namespace DemoAPIProject.Model;

/// <summary>
/// Represents a model class for storing a long URL and the creator of the shortened URL.
/// </summary>
public class ShortenUrl
{
    /// <summary>
    /// Gets or sets the long URL to be shortened.
    /// </summary>
    public string LongUrl { get; set; }

    /// <summary>
    /// Gets or sets the user who created the shortened URL.
    /// </summary>
    public string CreatedBy { get; set; }
}