namespace DemoAPIProject.Services
{
    using DemoAPIProject.Entity;
    using DemoAPIProject.Model;

    /// <summary>
    /// Interface defining user service operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Reads a CSV file from a stream and returns a paginated list of user records, filtered and sorted based on specified criteria.
        /// </summary>
        /// <param name="fileStream">The stream containing the CSV file to be read.</param>
        /// <param name="page">The page number for pagination (1-based).</param>
        /// <param name="pageSize">The number of records to return per page.</param>
        /// <param name="searchTerm">The term used to filter user records based on first name, last name, email, or job title.</param>
        /// <param name="sortField">The name of the property by which to sort the records.</param>
        /// <param name="isAscending">Indicates whether to sort the results in ascending order.</param>
        /// <param name="totalCount">Output parameter that returns the total count of user records matching the search criteria.</param>
        /// <returns>An enumerable collection of <see cref="UserModel"/> records that match the search criteria, sorted and paginated.</returns>
        public IEnumerable<UserModel> ReadCsvFile(Stream fileStream, int page, int pageSize, string searchTerm, string sortField, bool isAscending, out int totalCount);

        /// <summary>
        /// Deletes a user record from the CSV file based on the provided ID.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>true if the deletion was successful; otherwise, false.</returns>
        bool DeleteUserFromCsv(string filePath, string id);

        /// <summary>
        /// Retrieves a record by its unique identifier from a CSV file.
        /// </summary>
        /// <param name="idToFind">The unique identifier of the user to find.</param>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>The record as a string if found; otherwise, null.</returns>
        string GetRecordById(string idToFind, string filePath);

        /// <summary>
        /// Edits an existing user record in the CSV file based on the provided ID.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <param name="idToEdit">The unique identifier of the user to edit.</param>
        /// <param name="newData">The new data for the user record in CSV format.</param>
        void EditRecordInCsv(string filePath, string idToEdit, string newData);

        /// <summary>
        /// Adds a new user record to the CSV file.
        /// </summary>
        /// <param name="user">The <see cref="UserAddRequest"/> containing the user data.</param>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>true if the addition was successful; otherwise, false.</returns>
        bool AddUserInCsv(UserAddRequest user, string filePath);

        /// <summary>
        /// Asynchronously retrieves all users.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an
        /// enumerable collection of <see cref="User"/> objects.
        /// </returns>
        public Task<IEnumerable<User>> GetAllAsync();

    }
}