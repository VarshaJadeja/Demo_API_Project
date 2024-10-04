namespace DemoAPIProject.Model;

public class UserUpdateRequest
{
    public string Id { get; set; }
    public string NewData { get; set; }
}

public class UserAddRequest
{
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the lastname for the new user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// This property represents the user's email address.
    /// </summary>
    public string Email { get; set; }

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

}