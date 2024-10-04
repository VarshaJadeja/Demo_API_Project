namespace Account.Repositories
{
    using Account.Configuration;
    using Account.Entity;
    using Account.Model;
    using MongoDB.Driver;
    using System.Threading.Tasks;

    /// <summary>
    /// Repository class for handling User entities in MongoDB.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> collection;

        /// <summary>
        /// Initializes a new instance of the UserRepository class.
        /// </summary>
        /// <param name="context">The MongoDbContext instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="collectionName">The name of the collection (optional).</param>
        public UserRepository(MongoDbContext context, IConfiguration configuration, string? collectionName = null)
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(User).Name;
            }

            this.collection = context.GetCollection<User>(collectionName);
        }

        /// <summary>
        /// Retrieves a User entity based on the provided login request asynchronously.
        /// </summary>
        /// <param name="loginRequest">The login request containing username and password.</param>
        /// <returns>The User entity corresponding to the login request.</returns>
        public async Task<User> GetUser(LoginRequest loginRequest)
        {
            var filter = Builders<User>.Filter.And(
                    Builders<User>.Filter.Eq("UserName", loginRequest.UserName),
                    Builders<User>.Filter.Eq("Password", loginRequest.Password));

            var user = await this.collection.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// Checks if a user with the given username already exists.
        /// </summary>
        /// <param name="username">The username to check for uniqueness.</param>
        /// <returns>True if the username is unique, false otherwise.</returns>
        public bool IsUniqueUser(string username)
        {
            var filter = Builders<User>.Filter.Eq("UserName", username);
            var user = this.collection.Find(filter).FirstOrDefaultAsync();
            return user.Result == null;
        }

        /// <summary>
        /// Registers a new user based on the provided registration request asynchronously.
        /// </summary>
        /// <param name="registrationRequest">The registration request containing user details.</param>
        /// <returns>The registered User entity or null if the username is not unique.</returns>
        public async Task<User> Register(RegistrationRequest registrationRequest)
        {
            bool isUserNameUnique = this.IsUniqueUser(registrationRequest.UserName);
            if (!isUserNameUnique)
            {
                return null;
            }

            User user = new User
            {
                FirstName = registrationRequest.FirstName,
                LastName = registrationRequest.LastName,
                UserName = registrationRequest.UserName,
                Password = registrationRequest.Password,
                Email = registrationRequest.Email,
            };

            await this.collection.InsertOneAsync(user);
            user.Password = string.Empty;
            return user;
        }
    }
}