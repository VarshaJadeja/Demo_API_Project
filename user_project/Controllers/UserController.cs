namespace DemoAPIProject.Controllers
{
    using DemoAPIProject.Constants;
    using DemoAPIProject.Model;
    using DemoAPIProject.Services;
    using Microsoft.AspNetCore.Mvc;
    using static AuthService.Greeter;

    /// <summary>
    /// Controller responsible for user-related operations.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", "MOCK_DATA_1.csv");
        private readonly GreeterClient _client;
        /// <summary>
        /// Constructor for UserController class.
        /// </summary>
        /// <param name="userService">The service for user operations.</param>
        public UserController(IUserService userService, GreeterClient client)
        {
            this.userService = userService;
            _client = client;
        }

        /// <summary>
        /// Asynchronously authenticates a user based on the provided login request.
        /// </summary>
        /// <param name="model">The login request model containing user credentials.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an
        /// <see cref="IActionResult"/> representing the outcome of the login attempt,
        /// including success or failure messages.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthService.LoginRequest model)
        {
            try
            {
                // Call the Login method on the gRPC server
                var loginResponse = await _client.LoginAsync(model);

                if (loginResponse.User == null)
                {
                    return BadRequest("User not found.");
                }

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously registers a new user based on the provided registration request.
        /// </summary>
        /// <param name="registerRequest">The registration request model containing user information.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an
        /// <see cref="IActionResult"/> representing the outcome of the registration attempt,
        /// including success or failure messages.
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthService.RegisterRequest registerRequest)
        {
            // Validate input (you may want to add more validation here)
            if (registerRequest == null)
            {
                return BadRequest(ErrorMessages.InvalidRegistrationRequest);
            }

            // Call the gRPC Register method
            var response = await _client.RegisterAsync(registerRequest);

            // Check the response and return appropriate result
            if (response == null)
            {
                return BadRequest(ErrorMessages.UserAlreadyExists);
            }
            else
            {
                return Ok(response);
            }
        }

        /// <summary>
        /// Retrieves paginated user data from a CSV file, allowing for filtering and sorting.
        /// </summary>
        /// <param name="isAscending">Indicates whether the results should be sorted in ascending order. Defaults to true.</param>
        /// <param name="sortField">The field name to sort the user records by.</param>
        /// <param name="page">The page number for pagination (1-based). Defaults to 1.</param>
        /// <param name="pageSize">The number of records to return per page. Defaults to 10.</param>
        /// <param name="searchTerm">The term used to filter user records.</param>
        /// <returns>An <see cref="IActionResult"/> containing the total count of users and the paginated user data, or a Bad Request if no data is found.</returns>
        [HttpGet("userlist")]
        public IActionResult GetMockUserData(bool isAscending = true, string sortField = "", int page = 1, int pageSize = 10, string searchTerm = "")
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read);
            var paginatedUsers = this.userService.ReadCsvFile(stream, page, pageSize, searchTerm, sortField, isAscending, out int totalCount);
            if (paginatedUsers == null || !paginatedUsers.Any())
            {
                return BadRequest(ErrorMessages.NoDataFound);
            }

            return Ok(new
            {
                TotalCount = totalCount,
                Users = paginatedUsers
            });
        }

        /// <summary>
        /// Edits a user's data by their unique identifier.
        /// </summary>
        /// <param name="request">The <see cref="UserUpdateRequest"/> containing the user ID and new data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success of the operation.</returns>
        [HttpPost("EditDataById")]
        public IActionResult EditMockUserById([FromBody] UserUpdateRequest request)
        {
            this.userService.EditRecordInCsv(filePath, request.Id, request.NewData);
            return Ok();
        }

        /// <summary>
        /// Deletes a user based on their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success of the operation.</returns>
        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string id)
        {
            this.userService.DeleteUserFromCsv(filePath, id);
            return Ok();
        }

        /// <summary>
        /// Adds a new user to the CSV file.
        /// </summary>
        /// <param name="user">The <see cref="UserAddRequest"/> containing the user data to add.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success of the operation or a bad request response if the user data is null.</returns>
        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] UserAddRequest user)
        {
            if (user == null)
            {
                return BadRequest("User data is null");
            }

            this.userService.AddUserInCsv(user, filePath);
            return Ok();
        }

        [HttpGet("getAllUser")]
        public async Task<IActionResult> GetUser()
        {
            var list = await this.userService.GetAllAsync();
            return Ok(list);
        }
    }
}