namespace DemoAPIProject.Services
{
    using CsvHelper;
    using CsvHelper.TypeConversion;
    using DemoAPIProject.Entity;
    using DemoAPIProject.Model;
    using DemoAPIProject.Repositories;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Service class responsible for user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IRepository _repository;
        public UserService(IRepository repository) 
        {
            _repository = repository;
        }
        /// <summary>
        /// Reads a CSV file and returns a paginated list of user records filtered by a search term.
        /// </summary>
        /// <param name="fileStream">The stream of the CSV file to be read.</param>
        /// <param name="page">The page number for pagination (1-based).</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="searchTerm">The term used to filter user records.</param>
        /// <param name="sortField">The field name by which to sort the records.</param>
        /// <param name="isAscending">Indicates whether to sort in ascending order.</param>
        /// <param name="totalCount">Output parameter that returns the total count of filtered records.</param>
        /// <returns>An enumerable collection of <see cref="UserModel"/> records matching the search criteria, paginated according to the specified parameters.</returns>

        public IEnumerable<UserModel> ReadCsvFile(Stream fileStream, int page, int pageSize, string searchTerm, string sortField, bool isAscending, out int totalCount)
        {
            try
            {
                using (var reader = new StreamReader(fileStream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<UserModel>().ToList();

                    // Filter the users based on the search term
                    var filteredList = records.Where(user =>
                        user.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        user.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        user.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        user.JobTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    );

                    // Calculate total count before pagination
                    totalCount = filteredList.Count();

                    if (!string.IsNullOrEmpty(sortField))
                    {
                        var propertyInfo = typeof(UserModel).GetProperty(sortField);

                        if (propertyInfo == null)
                        {
                            throw new ArgumentException($"No property '{sortField}' found on type '{nameof(UserModel)}'.");
                        }
                        filteredList = isAscending
                            ? filteredList.OrderBy(u => propertyInfo.GetValue(u))
                            : filteredList.OrderByDescending(u => propertyInfo.GetValue(u));
                    }

                    // Pagination logic
                    var paginatedList = filteredList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    return paginatedList;
                }
            }
            catch (HeaderValidationException ex)
            {
                throw new ApplicationException("CSV file header is invalid.", ex);
            }
            catch (TypeConverterException ex)
            {
                throw new ApplicationException("CSV file contains invalid data format.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading CSV file", ex);
            }
        }

        /// <summary>
        /// Retrieves a record by its unique identifier from a CSV file.
        /// </summary>
        /// <param name="idToFind">The unique identifier of the user to find.</param>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>The record as a string if found; otherwise, null.</returns>
        public string GetRecordById(string idToFind, string filePath)
        {
            var lines = ReadCsvLines(filePath);

            var records = lines.Skip(1).ToList();

            foreach (var record in records)
            {
                if (record.StartsWith(idToFind + ","))
                {
                    return record;
                }
            }

            return null;
        }

        /// <summary>
        /// Edits an existing user record in the CSV file based on the provided ID.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <param name="idToEdit">The unique identifier of the user to edit.</param>
        /// <param name="newData">The new data for the user record in CSV format.</param>
        /// <exception cref="ApplicationException">Thrown when an error occurs while writing to the CSV file.</exception>
        public void EditRecordInCsv(string filePath, string idToEdit, string newData)
        {
            var lines = ReadCsvLines(filePath);

            // Assuming the first line is the header
            var header = lines.First();
            var records = lines.Skip(1).ToList();

            // Find the record to edit
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].StartsWith(idToEdit + ","))
                {
                    records[i] = newData;
                    break;
                }
            }

            WriteCsvFile(filePath, header, records);
            Console.WriteLine("Record edited successfully.");
        }

        /// <summary>
        /// Deletes a user record from the CSV file based on the provided ID.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>true if the deletion was successful; otherwise, false.</returns>
        public bool DeleteUserFromCsv(string filePath, string id)
        {
            var lines = ReadCsvLines(filePath);

            var header = lines.First();
            var records = lines.Skip(1).ToList();

            var updatedRecords = records.Where(line => !line.StartsWith(id + ",")).ToList();

            WriteCsvFile(filePath, header, updatedRecords);
            return true;
        }

        /// <summary>
        /// Adds a new user record to the CSV file.
        /// </summary>
        /// <param name="user">The <see cref="UserAddRequest"/> containing the user data.</param>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>true if the addition was successful; otherwise, false.</returns>
        public bool AddUserInCsv(UserAddRequest user, string filePath)
        {
            var newId = GenerateNewId(filePath);

            // Prepare CSV format
            var newUserRecord = $"{newId},{user.FirstName},{user.LastName},{user.Email},{user.JobTitle},{user.Title}\n";
            // Append the new user record to the CSV file
            System.IO.File.AppendAllText(filePath, newUserRecord, Encoding.UTF8);
            return true;
        }

        /// <summary>
        /// Generates a new unique identifier for the user based on existing records in the CSV file.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A new unique identifier as a string.</returns>
        private string GenerateNewId(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return "1"; // Start with ID 1 if the file does not exist
            }

            var lines = System.IO.File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                return "1"; // Start with ID 1 if the file is empty
            }

            // Get the highest existing ID
            var lastLine = lines.Last();
            var lastId = lastLine.Split(',')[0];
            return (int.Parse(lastId) + 1).ToString();
        }

        /// <summary>
        /// Reads all lines from the specified CSV file and returns them as a list of strings.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A list of strings containing the lines from the CSV file.</returns>
        private List<string> ReadCsvLines(string filePath)
        {
            return File.ReadAllLines(filePath).ToList();
        }

        /// <summary>
        /// Writes the specified header and records to the CSV file.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <param name="header">The header line to write to the CSV file.</param>
        /// <param name="records">The list of records to write to the CSV file.</param>
        /// <exception cref="IOException">Thrown when an error occurs while writing to the file.</exception>
        private void WriteCsvFile(string filePath, string header, List<string> records)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(header);
                foreach (var record in records)
                {
                    writer.WriteLine(record);
                }
            }
        }

        /// <summary>
        /// Asynchronously retrieves all users from the repository.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an
        /// enumerable collection of <see cref="User"/> objects.
        /// </returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list;
        }
    }
}