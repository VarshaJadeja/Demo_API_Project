namespace Account.Repositories
{
    using Account.Entity;
    using Account.Model;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface defining methods for user-related operations in the repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Asynchronously retrieves a user entity based on the provided login request.
        /// </summary>
        /// <param name="loginRequest">The login request containing username and password.</param>
        /// <returns>The user entity corresponding to the login request.</returns>
        Task<User> GetUser(LoginRequest loginRequest);

        /// <summary>
        /// Asynchronously registers a new user based on the provided registration request.
        /// </summary>
        /// <param name="registrationRequest">The registration request containing user details.</param>
        /// <returns>The registered user entity.</returns>
        Task<User> Register(RegistrationRequest registrationRequest);
    }
}