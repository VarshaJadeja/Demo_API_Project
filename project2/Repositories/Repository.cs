namespace DemoAPIProject.Repositories
{
    using DemoAPIProject.Configuration;
    using DemoAPIProject.Entity;
    using MongoDB.Driver;
    using System.Threading.Tasks;

    /// <summary>
    /// Repository class that provides data access methods for UrlMapping entities.
    /// </summary>
    public class Repository : IRepository
    {
        private readonly IMongoCollection<UrlMapping> collection;
        private readonly IMongoCollection<User> collection1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class with the specified context and collection name.
        /// </summary>
        /// <param name="context">The MongoDB context instance.</param>
        /// <param name="collectionName">The name of the collection (optional).</param>
        public Repository(MongoDbContext context, string? collectionName = null, string collectionName1 = null)
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(UrlMapping).Name;
                collectionName1 = typeof(User).Name;
            }

            this.collection = context.GetCollection<UrlMapping>(collectionName);
            this.collection1 = context.GetCollection<User>(collectionName1);
        }

        /// <summary>
        /// Retrieves a UrlMapping entity based on the provided short URL asynchronously.
        /// </summary>
        /// <param name="shortUrl">The short URL to search by.</param>
        /// <returns>The UrlMapping entity associated with the short URL.</returns>
        public async Task<UrlMapping> GetUrlByShortUrlAsync(string shortUrl)
        {
            return await this.collection.Find(url => url.ShortUrl == shortUrl).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new UrlMapping entity in the repository asynchronously.
        /// </summary>
        /// <param name="urlMapping">The UrlMapping entity to create.</param>
        public async Task CreateUrlMappingAsync(UrlMapping urlMapping)
        {
            await this.collection.InsertOneAsync(urlMapping);
        }

        /// <summary>
        /// Retrieves a UrlMapping entity based on the provided long URL asynchronously.
        /// </summary>
        /// <param name="longUrl">The long URL to search by.</param>
        /// <returns>The UrlMapping entity associated with the long URL.</returns>
        public async Task<UrlMapping> GetUrlMappingByLongUrlAsync(string longUrl)
        {
            return await this.collection.Find(url => url.LongUrl == longUrl).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates the visit count of a UrlMapping entity identified by the short URL asynchronously.
        /// </summary>
        /// <param name="shortUrl">The short URL of the UrlMapping entity to update.</param>
        public async Task UpdateUrlMappingAsync(string shortUrl)
        {
            var filter = Builders<UrlMapping>.Filter.Eq(url => url.ShortUrl, shortUrl);
            var update = Builders<UrlMapping>.Update.Inc(url => url.VisitCount, 1);
            await this.collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var cursor = await collection1.FindAsync(Builders<User>.Filter.Empty);
            return await cursor.ToListAsync();
        }
    }
}