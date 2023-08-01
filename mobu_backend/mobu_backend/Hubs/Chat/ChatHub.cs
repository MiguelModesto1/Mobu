using Microsoft.AspNetCore.SignalR;

namespace mobu_backend.Hubs.Chat
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task<string> WaitForMessage(string connectionId)
        {
            string message = await Clients.Client(connectionId).GetMessage();
            return message;
        }
        public async Task SendMessageToUser(string user, string message)
            => await Clients.User(user).ReceiveMessage(message);

        public async Task SendMessageToClient(string message, string connectionId)
            => await Clients.Client(connectionId).ReceiveMessage(message);

        public async Task AddToRoom(string roomName, string connectionId)
        {
            await Groups.AddToGroupAsync(connectionId, roomName);
        }

        public async Task RemoveFromRoom(string roomName, string connectionId)
        {
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
        }

        public async Task SendMessageToRoom(string message, string room)
            => await Clients.Group(room).ReceiveMessage(message);
    }
}