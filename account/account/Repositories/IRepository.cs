namespace DemoAPIProject.Repositories
{
    using DemoAPIProject.Entity;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface defining methods for interacting with UrlMapping entities.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Asynchronously retrieves a UrlMapping entity based on the provided short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to search for.</param>
        /// <returns>The UrlMapping entity associated with the short URL.</returns>
        Task<UrlMapping> GetUrlByShortUrlAsync(string shortUrl);

        /// <summary>
        /// Asynchronously creates a new UrlMapping entity in the repository.
        /// </summary>
        /// <param name="urlMapping">The UrlMapping entity to create.</param>
        Task CreateUrlMappingAsync(UrlMapping urlMapping);

        /// <summary>
        /// Asynchronously retrieves a UrlMapping entity based on the provided long URL.
        /// </summary>
        /// <param name="longUrl">The long URL to search for.</param>
        /// <returns>The UrlMapping entity associated with the long URL.</returns>
        Task<UrlMapping> GetUrlMappingByLongUrlAsync(string longUrl);

        /// <summary>
        /// Asynchronously updates the visit count of a UrlMapping entity identified by the short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL of the UrlMapping entity to update.</param>
        Task UpdateUrlMappingAsync(string shortUrl);

        public Task<IEnumerable<User>> GetAllAsync();
    }
}