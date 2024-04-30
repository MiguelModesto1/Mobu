using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using mobu_backend.Data;
using mobu_backend.Models;
using Org.BouncyCastle.Utilities.Encoders;

namespace mobu_backend.Hubs.Chat
{
    /// <summary>
    /// <para>Hub para comunicacao em tempo real</para>
    /// 
    /// <para>Métodos:</para>
    /// 
    /// <para>OnConnectedAsync() - callback de conexao;</para>
    /// <para>GenerateTokenFromHeader() - para geracao de token de conexao;</para>
    /// <para>WaitForMessage(string fromUser) - espera pela mensagem do utilizador 'fromUser';</para>
    /// <para>AddConnection(string roomId) - adiciona conexao a sala;</para>
    /// <para>RemoveConnection(string roomId) - remove conexao da sala;</para>
    /// <para>SendMessageToRoom(string fromUser, string roomId, string message) - envia mensagem para a sala;</para>
    /// <para>Block(string fromUser, string roomId) - bloqueia um utilizador;</para>
    /// <para>Unblock(string fromUser, string roomId) - desbloqueia um utilizador;</para>
    /// <para>EnterGroup(string fromUser, string group) - entrar no grupo 'group';</para>
    /// <para>LeaveGroup(string fromUser, string group) - deixar o grupo 'group';</para>
    /// <para>SendRequestToUser(string fromUser, string toUser) - envia pedido a 'toUser';</para>
    /// <para>SendRequestReply(string replier, string toUser, bool reply) - responde ao pedido de 'toUser';</para>
    ///
    /// </summary>
    [Authorize]
    public class RealTimeHub : Hub<IRealTimeHub>
    {

        private readonly ApplicationDbContext _context;

        public RealTimeHub(ApplicationDbContext context)
        {
            _context = context;
        }


        public override async Task OnConnectedAsync()
        {

            // Get Header Authorization value
            var httpCtx = Context.GetHttpContext();
            string header = httpCtx.Request.Headers.Authorization;
            string token = "";
            if (header != null && header.Length != 0)
                token = this.GenerateTokenFromHeader(header);

            await Clients.Client(Context.ConnectionId).OnConnectedAsyncPrivate("Entraste e o teu ID é o " + Context.ConnectionId);
        }

        private string GenerateTokenFromHeader(string header)
        {

            string decodedHeader = System.Text.Encoding.UTF8.GetString(Base64.Decode(header));

            return header;
        }

        public async Task<string> WaitForMessage(string fromUser)
            => await Clients.User(fromUser).GetMessage();

        public async Task AddConnection(string roomId)
        {
            if (!int.TryParse(roomId, out var roomIdOut))
            {
                // Handle invalid input here (e.g., log an error or return early).
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task RemoveConnection(string roomId)
        {
            if (!int.TryParse(roomId, out var roomIdOut))
            {
                // Handle invalid input here (e.g., log an error or return early).
                return;
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task SendMessageToRoom(string fromUser, string roomId, string message)
        {

            if (!int.TryParse(fromUser, out var fromUserId) || !int.TryParse(roomId, out var roomIdOut))
            {
                // Handle invalid input here (e.g., log an error or return early).
                return;
            }

            // sala
            IQueryable<int> salaIdQuery = _context.RegistadosSalasChat
            .Where(rs => rs.SalaFK == Convert.ToInt32(roomId) && rs.UtilizadorFK == fromUserId)
            .Select(rs => rs.SalaFK);

            int salaId = salaIdQuery.ToArray()[0];

            var msgQuery = _context.Mensagem.Where(m => m.SalaFK == salaId);

            int prevMensagemId = !msgQuery.IsNullOrEmpty() ? msgQuery.Max(m => m.IDMensagem) : 0;

            Mensagem msg = new()
            {
                IDMensagem = prevMensagemId + 1,
                ConteudoMsg = message,
                DataHoraMsg = DateTime.Now,
                RemetenteFK = fromUserId,
                SalaFK = salaId
            };

            //await _context.Mensagem.AddAsync(msg);
            _context.Attach(msg);
            await _context.SaveChangesAsync();

            await Clients.Group(roomId).ReceiveMessage(fromUserId, message);
        }

        public async Task Block(string fromUser, string toUser)
        {
            // verificação
            if (!int.TryParse(fromUser, out int fromUserId) || !int.TryParse(toUser, out int toUserId))
            {
                return;
            }

            // utilizador
            var user = _context.UtilizadorRegistado
                .Where(
                    a => a.IDUtilizador == fromUserId
                ).ToArray()[0];

            // amigo
            var friend = _context.UtilizadorRegistado
                .Where(
                    a => a.IDUtilizador == toUserId
                ).ToArray()[0];

            // amizade
            var friendship = _context.Amizade
                .Where(
                    f => f.RemetenteFK == user.IDUtilizador && f.DestinatarioFK == friend.IDUtilizador
                ).ToArray()[0];

            friendship.Desbloqueado = false;

            _context.Update(friendship);
            await _context.SaveChangesAsync();

            await Clients.User(toUser).ReceiveBlock(fromUser);

        }

        public async Task Unblock(string fromUser, string toUser)
        {
            // verificação
            if (!int.TryParse(fromUser, out int fromUserId) || !int.TryParse(toUser, out int toUserId))
            {
                return;
            }

            // utilizador
            var user = _context.UtilizadorRegistado.
                Where(
                    a => a.IDUtilizador == fromUserId
                )
                .ToArray()[0];

            // amigo
            var friend = _context.UtilizadorRegistado.
                Where(
                    a => a.IDUtilizador == toUserId
                )
                .ToArray()[0];

            // amizade
            var friendship = _context.Amizade
                .Where(
                    f => f.RemetenteFK == user.IDUtilizador && f.DestinatarioFK == friend.IDUtilizador
                ).ToArray()[0];

            friendship.Desbloqueado = true;

            _context.Update(friendship);
            await _context.SaveChangesAsync();

            await Clients.User(toUser).ReceiveUnblock(fromUser);

        }

        public async Task EnterGroup(string fromUser, string group)
        {

            if (!int.TryParse(fromUser, out var fromUserId) || !int.TryParse(group, out var groupId))
            {
                return;
            }

            RegistadosSalasChat registados = new()
            {
                IsAdmin = false,
                SalaFK = groupId,
                UtilizadorFK = fromUserId
            };

            _context.Attach(registados);
            await _context.SaveChangesAsync();

            await Groups.AddToGroupAsync(Context.ConnectionId, group);

            await Clients.Group(group).ReceiveEntry(fromUser + " juntou-se!");
        }

        public async Task LeaveGroup(string userRemoved, string group)
        {

            if (!int.TryParse(userRemoved, out var userRemovedId) || !int.TryParse(group, out var groupId))
            {
                return;
            }

            RegistadosSalasChat registados = await _context.RegistadosSalasChat
            .FirstOrDefaultAsync(rs => rs.SalaFK == groupId && rs.UtilizadorFK == userRemovedId);

            _context.Remove(registados);
            await _context.SaveChangesAsync();

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);

            await Clients.Group(group).ReceiveLeaving(userRemoved + " abandonou o grupo!");
        }

        public async Task SendRequestToUser(string fromUser, string toUser)
        {

            if (!int.TryParse(fromUser, out var fromUserId) || !int.TryParse(toUser, out var toUserId))
            {
                // Handle invalid input here (e.g., log an error or return early).
                return;
            }

            // remetente
            var from = _context.UtilizadorRegistado.Where(
                    u => u.IDUtilizador == fromUserId
                ).ToArray()[0];

            // destinatario
            var dest = _context.UtilizadorRegistado.Where(
                    u => u.IDUtilizador == toUserId
                ).ToArray()[0];

            // pedido

            var req = new Amizade()
            {
                DataPedido = DateTime.Now,
                Desbloqueado = true,
                Remetente = from,
                RemetenteFK = fromUserId,
                Destinatario = dest,
                DestinatarioFK = toUserId
            };

            _context.Attach(req);
            await _context.SaveChangesAsync();

            await Clients.User(toUser).ReceiveRequest(fromUser, dest.NomeUtilizador);
        }

        public async Task SendRequestReply(string replier, string toUser, bool reply)
        {
            if (!int.TryParse(replier, out var replierId) || !int.TryParse(toUser, out var toUserId))
            {
                return;
            }

            

            if (reply)
            {

                // remetente confirmacao
                var replierUser = _context.UtilizadorRegistado
                .Where(u => u.IDUtilizador == replierId)
                .ToArray()[0];
            
                // destinatario confirmacao
                var to = _context.UtilizadorRegistado
                .Where(u => u.IDUtilizador == toUserId)
                .ToArray()[0];

                // pedido
                var req = _context.Amizade
                    .Where(p => p.DestinatarioFK == replierId && p.RemetenteFK == toUserId)
                    .ToArray()[0];

                req.DataResposta = DateTime.Now;

                // amizades
                var friend = new Amizade()
                {
                    DataPedido = DateTime.Now,
                    DataResposta = DateTime.Now,
                    Desbloqueado = true,
                    Destinatario = to,
                    DestinatarioFK = toUserId,
                    Remetente = replierUser,
                    RemetenteFK = replierId
                };

                // sala

                SalasChat sala = new()
                {
                    NomeSala = to.NomeUtilizador + "_" + replierUser.NomeUtilizador,
                    SeGrupo = false,
                    NomeFotografia = "default_avatar.png",
                    DataFotografia = DateTime.Now
                };

                // criar a sala primeiro
                _context.Attach(sala);
                await _context.SaveChangesAsync();

                RegistadosSalasChat toUserRs = new()
                {
                    SalaFK = _context.SalasChat.Where(s => s.NomeSala == sala.NomeSala).Select(s => s.IDSala).ToArray()[0],
                    UtilizadorFK = toUserId
                };

                RegistadosSalasChat replierRs = new()
                {
                    SalaFK = _context.SalasChat.Where(s => s.NomeSala == sala.NomeSala).Select(s => s.IDSala).ToArray()[0],
                    UtilizadorFK = replierId
                };

                // ... depois criar o amigo
                _context.Update(req);
                _context.Attach(friend);
                _context.Attach(toUserRs);
                _context.Attach(replierRs);
                await _context.SaveChangesAsync();

            }
            await Clients.User(toUser).ReceiveRequestReply(replier, reply);
        }
    }
}