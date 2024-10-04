namespace DemoAPIProject.Configuration
{
    using MongoDB.Driver;

    /// <summary>
    /// Represents a MongoDB database context that provides access to collections.
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbContext"/> class with the specified MongoDB database.
        /// </summary>
        /// <param name="database">The MongoDB database instance.</param>
        public MongoDbContext(IMongoDatabase database)
        {
            this.database = database;
        }

        /// <summary>
        /// Gets a specific collection from the MongoDB database based on the entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the collection.</typeparam>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>The MongoDB collection for the specified entity type.</returns>
        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
            => this.database.GetCollection<TEntity>(collectionName);
    }
}
