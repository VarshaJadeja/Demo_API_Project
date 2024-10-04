namespace Account.Configuration
{
    /// <summary>
    /// Represents the settings required for connecting to the MongoDB database.
    /// </summary>
    public class DbSetting
    {
        /// <summary>
        /// Gets or sets the connection string for the MongoDB database.
        /// </summary>
        public required string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the MongoDB database.
        /// </summary>
        public required string DatabaseName { get; set; }
    }
}
