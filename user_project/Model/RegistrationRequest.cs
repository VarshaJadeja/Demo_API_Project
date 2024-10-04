namespace Account.Model;

/// <summary>
/// Represents the data required for user registration.
/// </summary>
public class RegistrationRequest
{
    /// <summary>
    /// Gets or sets the firstname for the new user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the lastname for the new user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the username for the new user.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password for the new user.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the confirm password for the new user.
    /// </summary>
    public string ConfirmPassword { get; set; }

    /// <summary>
    /// Gets or sets the email address for the new user.
    /// </summary>
    public string Email { get; set; }
}
