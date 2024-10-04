using Amazon.Runtime.Internal;

namespace DemoAPIProject.Constants
{
    /// <summary>
    /// Contains constant string values for error and status messages used in the application.
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Message indicating that a user already exists in the system.
        /// </summary>
        public const string UserAlreadyExists = "User already exists";

        /// <summary>
        /// Message indicating an error occurred during user registration.
        /// </summary>
        public const string ErrorWhileRegistering = "Error while registering";

        /// <summary>
        /// Message indicating that a user could not be found in the system.
        /// </summary>
        public const string UserNotFound = "User not found";

        /// <summary>
        /// Message indicating that a data could not be found in the system.
        /// </summary>
        public const string NoDataFound = "Data not found";

        /// <summary>
        /// Message indicating invalid credentials were provided.
        /// </summary>
        public const string InvalidCredentials = "Invalid Credentials";

        /// <summary>
        /// General success message.
        /// </summary>
        public const string UrlNotFound = "Url not Found";

        /// <summary>
        /// Message indicating that a user has been successfully registered.
        /// </summary>
        public const string RegisteredSuccessfully = "User Registered Successfully";

        public const string InvalidRegistrationRequest = "Invalid registration request.";

    }
}
