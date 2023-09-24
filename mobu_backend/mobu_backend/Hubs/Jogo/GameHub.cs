using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using mobu_backend.Data;
using mobu_backend.Models;

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
        public async Task<string> WaitForGameRoomState(string connectionId) 
            => await Clients.Client(connectionId).GetGameRoomState();
        
        public async Task SendGameRoomStateToUser(string toUser, string gameRoomState)
            => await Clients.User(toUser).ReceiveGameRoomState(gameRoomState);

        public async Task SendChallengeToUser(string fromUser, string toUser, string username) 
            => await Clients.User(toUser).ReceiveChallenge(fromUser, username);

        public async Task SendChallengeReply(string replier, string challenger, bool interested)
        {

            if(interested){

                //verificar se sala existe

                int [] rep = _context.Registados_Salas_Jogo
                .Where(rs => rs.UtilizadorFK == int.Parse(replier))
                .Select(rs => rs.SalaFK)
                .ToArray();

                int [] chal = _context.Registados_Salas_Jogo
                .Where(rs => rs.UtilizadorFK == int.Parse(challenger))
                .Select(rs => rs.SalaFK)
                .ToArray();

                if(rep.Intersect(chal) == new int[0]){
                    Sala_Jogo_1_Contra_1 salaJogo = new();
                    Registados_Salas_Jogo desafiador = new(){
                        UtilizadorFK = int.Parse(challenger),
                        IsFundador = true,
                        SalaFK = salaJogo.IDSala
                    };
                    Registados_Salas_Jogo desafiado = new(){
                        UtilizadorFK = int.Parse(replier),
                        IsFundador = false,
                        SalaFK = salaJogo.IDSala
                    };

                    _context.Attach(salaJogo);
                    _context.Attach(desafiador);
                    _context.Attach(desafiado);
                    await _context.SaveChangesAsync();
                }
            }

            await Clients.User(challenger).ReceiveReply(replier, interested);
        }

        /*public async Task AddToGameRoom(string roomId, string connectionId) 
            => await Groups.AddToGroupAsync(connectionId, "gameroom"+roomId);

        public async Task RemoveFromGameRoom(string roomId, string connectionId) 
            => await Groups.RemoveFromGroupAsync(connectionId, "gameroom"+roomId);*/
    }
}
