namespace DemoAPIProject.Model;

/// <summary>
/// Represents the request data required for user login.
/// Contains the credentials needed to authenticate a user.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// This property is used to identify the user attempting to log in.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// This property is used to authenticate the user attempting to log in.
    /// </summary>
    public string Password { get; set; }
}
