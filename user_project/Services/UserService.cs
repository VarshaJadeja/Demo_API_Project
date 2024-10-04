namespace Account.Services
{
    using Account.Repositories;
    using Grpc.Core;
    using aM = Account.Model;
    using aN = Account.Entity;
    using aT = AuthService;

    /// <summary>
    /// Service class responsible for user-related operations.
    /// </summary>
    public class UserService : aT.Greeter.GreeterBase
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Constructor for UserService class.
        /// </summary>
        /// <param name="userRepository">The repository for user data.</param>
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handles user login functionality.
        /// </summary>
        /// <param name="loginRequest">The login request containing user credentials.</param>
        /// <returns>Returns a LoginResponse object if the user is found, otherwise null.</returns>
        public async override Task<aT.LoginResponse> Login(aT.LoginRequest loginRequest, ServerCallContext context)
        {
            var request = new aM.LoginRequest
            {
                UserName = loginRequest.Username,
                Password = loginRequest.Password
            };
            aN.User user = await this.userRepository.GetUser(request);
            if (user == null)
            {
                return new aT.LoginResponse { User = null };
            }

            return new aT.LoginResponse
            {
                User = new aT.User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Email = user.Email
                }
            };
        }
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The registration request containing user information.</param>
        /// <returns>Returns the registered User object.</returns>
        public async override Task<aT.RegisterResponse> Register(aT.RegisterRequest request, ServerCallContext context)
        {
            var registrationRequest = new aM.RegistrationRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                Email = request.Email
            };
            var response = await this.userRepository.Register(registrationRequest);
            return new aT.RegisterResponse
            {
                User = new aT.User
                {
                    Id = response.Id,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Username = response.UserName,
                    Email = response.Email
                }
            };
        }
    }
}