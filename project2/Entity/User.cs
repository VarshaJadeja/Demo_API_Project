namespace DemoAPIProject.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Represents a user entity in the MongoDB database.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// This property is annotated with <see cref="BsonId"/> to mark it as the primary key in MongoDB.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the firstname for the new user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the lastname for the new user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// This property represents the user's login name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// This property stores the user's password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// This property represents the user's email address.
    /// </summary>
    public string Email { get; set; }
}
