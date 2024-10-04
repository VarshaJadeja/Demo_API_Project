namespace DemoAPIProject.Model;
/// <summary>
/// Represents a user entity in the MongoDB database.
/// </summary>
public class UserModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
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
    /// Gets or sets the job title of the user.
    /// This property stores the user's job title.
    /// </summary>
    public string JobTitle { get; set; }

    /// <summary>
    /// Gets or sets the title of the user.
    /// This property stores the user's title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// This property represents the user's email address.
    /// </summary>
    public string Email { get; set; }
}
