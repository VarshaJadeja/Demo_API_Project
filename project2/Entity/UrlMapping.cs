namespace DemoAPIProject.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Represents a mapping between a long URL and its corresponding short URL.
/// </summary>
public class UrlMapping
{
    /// <summary>
    /// Gets or sets the unique identifier for the URL mapping.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the long URL to be mapped.
    /// </summary>
    public string LongUrl { get; set; }

    /// <summary>
    /// Gets or sets the corresponding short URL.
    /// </summary>
    public string ShortUrl { get; set; }

    /// <summary>
    /// Gets or sets the number of visits to the short URL.
    /// </summary>
    public int VisitCount { get; set; }

    /// <summary>
    /// Gets or sets the user who created the mapping.
    /// </summary>
    public string CreatedBy { get; set; }
}