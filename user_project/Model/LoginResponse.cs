namespace Account.Model;
using Account.Entity;

/// <summary>
/// Represents the response returned upon a successful user login.
/// Contains the user information.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Gets or sets the user details.
    /// This property contains the information of the logged-in user.
    /// </summary>
    public User User { get; set; }
}
