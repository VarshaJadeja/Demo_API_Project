using DemoAPIProject.Configuration;
using DemoAPIProject.Entity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace DemoAPIProject.ChatHub
{
    public class ChatHub : Hub
    {
        private readonly IMongoCollection<ChatMessages> _chatMessages;
        private static readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

        public ChatHub(MongoDbContext mongoDbContext)
        {
            _chatMessages = mongoDbContext.GetCollection<ChatMessages>("ChatMessages");
        }

        public override async Task OnConnectedAsync()
        {
            // Assuming the user ID is passed in as a query string parameter when connecting
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                // Associate the connection ID with the user ID
                _userConnections[Context.ConnectionId] = userId;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Remove the connection when the user disconnects
            _userConnections.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string senderId, string recipientId, string message)
        {
            var chatMessage = new ChatMessages
            {
                SenderId = senderId,
                RecipientId = recipientId,
                Message = message
            };

            await _chatMessages.InsertOneAsync(chatMessage);
            // Find all connection IDs for the recipient user
            var recipientConnections = _userConnections
                .Where(kvp => kvp.Value == recipientId)
                .Select(kvp => kvp.Key)
                .ToList();

            // Send the message to each connection of the recipient
            foreach (var connectionId in recipientConnections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, message);
            }
        }
        public async Task GetChatHistory(string userId, string senderid)
        {
            var chatHistory = await _chatMessages
                .Find(m => (m.SenderId == senderid && m.RecipientId == userId) ||
                           (m.SenderId == userId && m.RecipientId == senderid))
                .ToListAsync();

            await Clients.Caller.SendAsync("ReceiveChatHistory", chatHistory);
        }
    }
}
