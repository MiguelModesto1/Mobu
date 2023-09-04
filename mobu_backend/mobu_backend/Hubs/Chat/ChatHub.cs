using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using mobu_backend.Data;

namespace mobu_backend.Hubs.Chat
{
    [Authorize(Roles = "Registered")]
    public class ChatHub : Hub<IChatClient>
    {

        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> WaitForMessage(string fromUser)
            => await Clients.User(fromUser).GetMessage();
        
        public async Task SendMessageToUser(string fromUser, string toUser, Message message)
            => await Clients.User(toUser).ReceiveMessage(fromUser, message);

        public async Task AddToRoom(string roomId, string connectionId) 
            => await Groups.AddToGroupAsync(connectionId, roomId);

        public async Task RemoveFromRoom(string roomId, string connectionId) 
            => await Groups.RemoveFromGroupAsync(connectionId, roomId);

        public async Task SendMessageToRoom(string fromUser, string roomId, Message message)
            => await Clients.Group(roomId).ReceiveMessage(fromUser, message);
    }
}