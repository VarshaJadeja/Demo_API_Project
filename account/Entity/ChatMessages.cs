namespace DemoAPIProject.Entity
{
    /// <summary>
    /// Represents a chat message in the system.
    /// </summary>
    public class ChatMessages
    {
        /// <summary>
        /// Gets or sets the unique identifier for the chat message.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the identifier of the user who sent the message.
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who is the recipient of the message.
        /// </summary>
        public string RecipientId { get; set; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the message was sent.
        /// The default value is the current UTC time.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

