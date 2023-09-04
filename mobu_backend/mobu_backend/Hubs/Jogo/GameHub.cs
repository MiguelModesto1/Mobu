using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using mobu_backend.Data;

namespace mobu_backend.Hubs.Jogo
{
    [AllowAnonymous]
    public class GameHub : Hub<IGameHub>
    {

        private readonly ApplicationDbContext _context;

        public GameHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<GameRoomState> WaitForGameRoomState(string connectionId) 
            => await Clients.Client(connectionId).GetGameRoomState();
        
        public async Task SendGameRoomStateToUser(string toUser, GameRoomState gameRoomState)
            => await Clients.User(toUser).ReceiveGameRoomState(gameRoomState);

        public async Task SendChallengeToUser(string fromUser, string toUser, bool interested)
            => await Clients.User(toUser).ReceiveChallenge(fromUser, interested);

        public async Task AddToGameRoom(string roomName, string connectionId) 
            => await Groups.AddToGroupAsync(connectionId, roomName);

        public async Task RemoveFromGameRoom(string roomName, string connectionId) 
            => await Groups.RemoveFromGroupAsync(connectionId, roomName);
    }
}
