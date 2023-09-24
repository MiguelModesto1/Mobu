using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu_backend.Hubs.Pedidos
{
    [Authorize(Roles = "Registered")]
    public class RequestHub : Hub<IRequestHub>
    {

        private readonly ApplicationDbContext _context;

        public RequestHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> WaitForRequestState(string fromUser)
           => await Clients.User(fromUser).GetRequestState();

        public async Task SendRequestToUser(string fromUser, string toUser)
        {

            if (!int.TryParse(fromUser, out var fromUserId) || !int.TryParse(toUser, out var toUserId))
            {
                // Handle invalid input here (e.g., log an error or return early).
                return;
            }

            Destinatario_Pedidos_Amizade dest = new(){
            RemetenteFK = fromUserId,
            IDDestinatarioPedido = toUserId,
            DataHoraPedido = DateTime.Now
            };

            _context.Attach(dest);
            await _context.SaveChangesAsync();
            
            await Clients.User(toUser).ReceiveRequest(fromUser, dest.RemetentePedido.NomeUtilizador);
        }

        public async Task SendRequestReply(string replier, string toUser, bool reply)
        {
            if (!int.TryParse(replier, out var replierId) || !int.TryParse(toUser, out var toUserId))
            {
                return;
            }

            Destinatario_Pedidos_Amizade dest = await _context.Destinatario_Pedidos_Amizade
            .FirstOrDefaultAsync
                (
                    d => d.RemetenteFK == int.Parse(replier) && d.IDDestinatarioPedido == int.Parse(toUser)
                );
            _context.Remove(dest);

            if(reply){

                // users
                string toUsername = _context.Utilizador_Registado
                .FirstOrDefaultAsync(u => u.IDUtilizador == int.Parse(toUser))
                .Result.NomeUtilizador;

                string replierUsername = _context.Utilizador_Registado
                .FirstOrDefaultAsync(u => u.IDUtilizador == int.Parse(replier))
                .Result.NomeUtilizador;

                // amigo

                Amigo amigo = new(){
                    IDAmigo = replierId,
                    DonoListaFK = toUserId
                };

                // sala

                Salas_Chat sala = new(){
                    NomeSala = toUsername + "_" + replierUsername,
                    SeGrupo = false,
                    NomeFotografia = "default_avatar.png",
                    DataFotografia = DateTime.Now
                };

                _context.Attach(amigo);
                _context.Attach(sala);
                await _context.SaveChangesAsync();

                Registados_Salas_Chat toUserRs = new(){
                    SalaFK = sala.IDSala,
                    UtilizadorFK = int.Parse(toUser)
                };

                Registados_Salas_Chat replierRs = new(){
                    SalaFK = sala.IDSala,
                    UtilizadorFK = int.Parse(replier)
                };

                _context.Attach(toUserRs);
                _context.Attach(replierRs);
                await _context.SaveChangesAsync();

            } 
            await Clients.User(toUser).ReceiveRequestReply(replier, reply);
        }

        public async Task EnterGroup(string fromUser, string group){

            if (!int.TryParse(fromUser, out var fromUserId) || !int.TryParse(group, out var groupId))
            {
                return;
            }

            Registados_Salas_Chat registados = new(){
                IsAdmin = false,
                SalaFK = groupId,
                UtilizadorFK = fromUserId
            };

            _context.Attach(registados);
            await _context.SaveChangesAsync();
        }

        public async Task LeaveGroup(string userRemoved, string group){

            if (!int.TryParse(userRemoved, out var userRemovedId) || !int.TryParse(group, out var groupId))
            {
                return;
            }

            Registados_Salas_Chat registados = await _context.Registados_Salas_Chat
            .FirstOrDefaultAsync(rs => rs.SalaFK == groupId && rs.UtilizadorFK == userRemovedId);

            _context.Remove(registados);
            await _context.SaveChangesAsync();

        }
    }
}
