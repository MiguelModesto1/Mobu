using Microsoft.AspNetCore.SignalR;

namespace mobu_backend.Hubs.Jogo
{
    public class GameHub : Hub<IGameHub>
    {
        public async Task<GameRoomState> WaitForGameRoomState(string connectionId)
        {
            GameRoomState state  = await Clients.Client(connectionId).GetGameRoomState();
            return state;
        }
        public async Task SendGameRoomStateToUser(string user, GameRoomState gameRoomState)
            => await Clients.User(user).ReceiveGameRoomState(gameRoomState);

        public async Task SendgameRoomStateToClient(string connectionId, GameRoomState gameRoomState)
            => await Clients.Client(connectionId).ReceiveGameRoomState(gameRoomState);

        public async Task AddToGameRoom(string roomName, string connectionId)
        {
            await Groups.AddToGroupAsync(connectionId, roomName);
        }

        public async Task RemoveFromGameRoom(string roomName, string connectionId)
        {
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
        }
    }
}
