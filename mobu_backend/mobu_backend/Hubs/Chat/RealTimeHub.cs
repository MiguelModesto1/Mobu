using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using mobu_backend.ApiModels;
using mobu_backend.Data;
using mobu_backend.Hubs.Connections;
using mobu_backend.Hubs.Objects;
using mobu_backend.Models;
//using Org.BouncyCastle.Utilities.Encoders;

namespace mobu_backend.Hubs.Chat
{
    /// <summary>
    /// Esta classe implementa um hub em tempo real para gerir a comunicação entre o cliente e o servidor.
    /// </summary>
    [Authorize(Roles = "Mobber")]
    [ValidateAntiForgeryToken]
    public class RealTimeHub : Hub<IRealTimeHub>
    {
        /// <summary>
        /// Contexto da base de dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Interface para a funcao de logging no controller
        /// </summary>
        private readonly ILogger<RealTimeHub> _logger;

        /// <summary>
        /// mapeamento de utilizadores
        /// </summary>
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        /// <summary>
        /// Construtor da classe RealTimeHub.
        /// </summary>
        /// <param name="context">O contexto do banco de dados a ser usado pela classe.</param>
        public RealTimeHub(
            ApplicationDbContext context,
            ILogger<RealTimeHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Método assíncrono que é invocado quando uma conexão é estabelecida.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public override async Task OnConnectedAsync()
        {

            // Obtém o valor do cabeçalho de autorização
            //var httpCtx = Context.GetHttpContext();
            //string header = httpCtx.Request.Headers.Authorization;
            //string token = "";

            //if (header != null && header.Length != 0)
            //{
            //    token = GenerateTokenFromHeader(header);
            //}

            //// cookie com ID de sessao
            //Context.GetHttpContext().Request.Cookies.TryGetValue("Session-Id", out var sesisonId);

            //// encontrar utilizador com o ID de sessao
            //var sessionClaim = await _context.UserClaims
            //    .FirstOrDefaultAsync(u => u.ClaimValue == sesisonId);

            //if (sessionClaim == null)
            //{
            //    Context.Abort();
            //    return;
            //}

            //var identityUser = await _context.Users
            //        .FirstOrDefaultAsync(u => u.Id == sessionClaim.UserId);
            //var user = await _context.UtilizadorRegistado
            //    .FirstOrDefaultAsync(u => u.AuthenticationID == identityUser.Id);

            //if (identityUser == null || user == null)
            //{
            //    Context.Abort();
            //    return;
            //}

            _connections.Add(Context.UserIdentifier, Context.ConnectionId);

            //await Clients.User(Context.UserIdentifier).OnConnectedAsyncPrivate("Entraste e o teu ID é o " + Context.ConnectionId);
            await Clients.Client(Context.ConnectionId).OnConnectedAsyncPrivate("Entraste e o teu ID é o " + Context.UserIdentifier);
        }

        /// <summary>
        /// Método assíncrono que é invocado quando uma conexão é desestabelecida.
        /// </summary>
        /// <param name="ex">(Exceção não utilizada)</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {

            await Clients.Client(Context.ConnectionId).OnDisconnectedAsyncPrivate("Saíste!");
            
            _connections.Remove(Context.UserIdentifier, Context.ConnectionId);

            Context.Abort();
        }

        ///// <summary>
        ///// Gera um token a partir do cabeçalho de autorização.
        ///// </summary>
        ///// <param name="header">O cabeçalho de autorização.</param>
        ///// <returns>O token gerado a partir do cabeçalho.</returns>
        //private string GenerateTokenFromHeader(string header)
        //{
        //    string decodedHeader = System.Text.Encoding.UTF8.GetString(Base64.Decode(header));
        //    return decodedHeader;
        //}

        /// <summary>
        /// Aguarda a chegada de uma mensagem de um determinado utilizador.
        /// </summary>
        /// <param name="fromUser">O ID do utilizador de origem.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona que retorna a mensagem.</returns>
        public async Task<string> WaitForMessage(string fromUser)
            => await Clients.User(fromUser).GetMessage();

        /// <summary>
        /// Adiciona uma conexão a um grupo específico.
        /// </summary>
        /// <param name="roomId">O ID do grupo a que a conexão será adicionada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task AddConnection(string roomId)
        {
            if (!int.TryParse(roomId, out var roomIdOut))
            {
                // Tratar entrada inválida aqui.
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).AddedToGroup(Context.ConnectionId, roomId);
        }

        /// <summary>
        /// Remove uma conexão de um grupo específico.
        /// </summary>
        /// <param name="roomId">O ID do grupo de que a conexão será removida.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task RemoveConnection(string roomId)
        {
            if (!int.TryParse(roomId, out var roomIdOut))
            {
                // Tratar entrada inválida aqui.
                return;
            }
            await Clients.Group(roomId).RemovedFromGroup(Context.ConnectionId, roomId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        /// <summary>
        /// Envia uma mensagem a um grupo específico.
        /// </summary>
        /// <param name="fromUser">O ID do utilizador de origem.</param>
        /// <param name="roomId">O ID do grupo a que a mensagem será enviada.</param>
        /// <param name="message">A mensagem a ser enviada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task SendMessageToRoom(string fromUser, string roomId, string message)
        {
            if (!int.TryParse(fromUser, out var fromUserId) || !int.TryParse(roomId, out var roomIdOut))
            {
                // Tratar entrada inválida aqui.
                return;
            }

            // Recupera dados da sala e do remetente
            var salaQuery = _context.RegistadosSalasChat
                .Where(rs => rs.SalaFK == Convert.ToInt32(roomId) && rs.UtilizadorFK == fromUserId);

            int salaId = salaQuery.Select(rs => rs.SalaFK).ToArray()[0];
            var sala = salaQuery.Select(rs => rs.Sala).ToArray()[0];

            // Obtém o ID da última mensagem existente
            int prevMensagemId = !_context.Mensagem.IsNullOrEmpty() ?
                _context.Mensagem.Max(m => m.IDMensagem) : 0;

            var msgUser = _context.UtilizadorRegistado
                .Where(m => m.IDUtilizador == fromUserId)
                .ToArray()[0];

            // Cria nova mensagem
            Mensagem msg = new()
            {
                IDMensagem = prevMensagemId + 1,
                ConteudoMsg = message,
                DataHoraMsg = DateTime.Now,
                RemetenteFK = fromUserId,
                Remetente = msgUser,
                SalaFK = salaId,
                Sala = sala
            };

            // Adiciona a mensagem ao contexto e salva
            try
            {
                _context.Add(msg);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            // Cria um objeto de mensagem para envio ao cliente
            var messageObject = new Messages()
            {
                IDSala = msg.SalaFK,
                IDMensagem = msg.IDMensagem,
                IDRemetente = msg.RemetenteFK,
                URLImagemRemetente = $"{Context.GetHttpContext().Request.Scheme}://{Context.GetHttpContext().Request.Host}" + "/imagens/" + msg.Remetente.NomeFotografia,
                NomeRemetente = msg.Remetente.NomeUtilizador,
                ConteudoMsg = msg.ConteudoMsg
            };

            // Envia a mensagem ao grupo
            await Clients.Group(roomId).ReceiveMessage(sala.SeGrupo, messageObject);
            //await Clients.User(Context.UserIdentifier).ReceiveMessage(sala.SeGrupo, messageObject);
        }

        /// <summary>
        /// Bloqueia um utilizador para outro utilizador.
        /// </summary>
        /// <param name="fromUser">O ID do utilizador que está a bloquear.</param>
        /// <param name="toUser">O ID do utilizador que está a ser bloqueado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task Block(string fromUser, string toUser)
        {
            // Verifica e converte os IDs dos utilizadores
            if (!int.TryParse(fromUser, out int fromUserId) || !int.TryParse(toUser, out int toUserId))
            {
                return;
            }

            // Busca os utilizadores a serem bloqueados
            var user = _context.UtilizadorRegistado
                .Where(a => a.IDUtilizador == fromUserId)
                .ToArray()[0];

            var friend = _context.UtilizadorRegistado
                .Where(a => a.IDUtilizador == toUserId)
                .ToArray()[0];

            // Encontra a amizade entre os utilizadores
            var friendship = _context.Amizade
                .Where(f => f.RemetenteFK == user.IDUtilizador && f.DestinatarioFK == friend.IDUtilizador)
                .ToArray()[0];

            // Atualiza a amizade para bloquear
            friendship.Desbloqueado = false;

            _context.Update(friendship);
            await _context.SaveChangesAsync();

            // Notifica o utilizador bloqueado
            var connections = _connections.GetConnections(friend.AuthenticationID);
            foreach (var connection in connections)
            {
                await Clients.Client(connection).ReceiveBlock(fromUser);
            }

            // Notifica o utilizador bloqueador
            await Clients.User(Context.UserIdentifier).ReceiveBlock(fromUser);
        }

        /// <summary>
        /// Desbloqueia um utilizador para outro utilizador.
        /// </summary>
        /// <param name="fromUser">O ID do utilizador que está desbloquear.</param>
        /// <param name="toUser">O ID do utilizador que está ser desbloqueado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task Unblock(string fromUser, string toUser)
        {
            // Verifica e converte os IDs dos utilizadores
            if (!int.TryParse(fromUser, out int fromUserId) || !int.TryParse(toUser, out int toUserId))
            {
                return;
            }

            // Busca os utilizadores a serem desbloqueados
            var user = _context.UtilizadorRegistado
                .Where(a => a.IDUtilizador == fromUserId)
                .ToArray()[0];

            var friend = _context.UtilizadorRegistado
                .Where(a => a.IDUtilizador == toUserId)
                .ToArray()[0];

            // Encontra a amizade entre os utilizadores
            var friendship = _context.Amizade
                .Where(f => f.RemetenteFK == user.IDUtilizador && f.DestinatarioFK == friend.IDUtilizador)
                .ToArray()[0];

            // Atualiza a amizade para desbloquear
            friendship.Desbloqueado = true;

            _context.Update(friendship);
            await _context.SaveChangesAsync();

            // Notifica o utilizador desbloqueado
            var connections = _connections.GetConnections(friend.AuthenticationID);
            foreach (var connection in connections)
            {
                await Clients.Client(connection).ReceiveUnblock(fromUser);
            }

            // Notifica o utilizador desbloqueador
            await Clients.User(Context.UserIdentifier).ReceiveUnblock(fromUser);
        }

        /// <summary>
        /// Notifica um grupo sobre a entrada de um utilizador.
        /// </summary>
        /// <param name="fromUser">O ID do utilizador que entrou.</param>
        /// <param name="group">O ID do grupo a que o utilizador se juntou.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task EnterGroup(string fromUser, string group)
        {
            if (!int.TryParse(fromUser, out var fromUserId) || !int.TryParse(group, out var groupId))
            {
                return;
            }

            // Adiciona o utilizador ao grupo
            RegistadosSalasChat registados = new()
            {
                IsAdmin = false,
                SalaFK = groupId,
                UtilizadorFK = fromUserId
            };

            _context.Add(registados);
            await _context.SaveChangesAsync();

            // Notifica o grupo sobre a entrada do utilizador
            await Clients.Group(group).ReceiveEntry(group, fromUser + " juntou-se!");

            // Notifica o grupo sobre a entrada do utilizador
            await Clients.User(Context.UserIdentifier).ReceiveEntry(group, fromUser + " juntou-se!");
        }

        /// <summary>
        /// Notifica um grupo sobre a saída de um utilizador.
        /// </summary>
        /// <param name="userRemoved">O ID do utilizador que saiu.</param>
        /// <param name="group">O ID do grupo de que o utilizador saiu.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task LeaveGroup(string userRemoved, string group)
        {
            if (!int.TryParse(userRemoved, out var userRemovedId) || !int.TryParse(group, out var groupId))
            {
                return;
            }

            // Remove o utilizador do grupo
            RegistadosSalasChat registados = await _context.RegistadosSalasChat
                .FirstOrDefaultAsync(rs => rs.SalaFK == groupId && rs.UtilizadorFK == userRemovedId);

            foreach (var connectionId in _connections.GetConnections(Context.UserIdentifier))
            {
                await Groups.RemoveFromGroupAsync(connectionId, group);
            }

            // Notifica o grupo sobre a saída do utilizador
            await Clients.Group(group).ReceiveLeaving(group, userRemoved + " abandonou o grupo!");

            //Notifica o cliente sobre a sua saída
            await Clients.User(Context.UserIdentifier).ReceiveLeaving(group, "Abandonou o grupo " + group);

            _context.Remove(registados);
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// Envia um pedido de amizade a um utilizador.
        /// </summary>
        /// <param name="fromUser">O ID do utilizador que está a enviar o pedido.</param>
        /// <param name="toUser">O ID do utilizador que está a receber o pedido.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task SendRequestToUser(string fromUser, string toUser)
        {
            if (!int.TryParse(fromUser, out var fromUserId) || !int.TryParse(toUser, out var toUserId))
            {
                // Tratar entrada inválida aqui.
                return;
            }

            // Busca os utilizadores remetente e destinatário
            var from = _context.UtilizadorRegistado
                .Where(u => u.IDUtilizador == fromUserId)
                .ToArray()[0];

            var dest = _context.UtilizadorRegistado
                .Where(u => u.IDUtilizador == toUserId)
                .ToArray()[0];

            try
            {
                // Cria um novo pedido de amizade
                var req = new Amizade()
                {
                    DataPedido = DateTime.Now,
                    Desbloqueado = true,
                    Remetente = from,
                    RemetenteFK = fromUserId,
                    Destinatario = dest,
                    DestinatarioFK = toUserId
                };

                _context.Add(req);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }


            // Notifica o remetente sobre o pedido de amizade
            await Clients.User(Context.UserIdentifier).ReceiveRequest(toUser, dest.NomeUtilizador);

            // Notifica o destinatário sobre o pedido de amizade
            await Clients.User(dest.AuthenticationID).ReceiveRequest(fromUser, dest.NomeUtilizador);
        }

        /// <summary>
        /// Envia uma resposta a um pedido de amizade de um utilizador.
        /// </summary>
        /// <param name="replier">O ID do utilizador que está a responder.</param>
        /// <param name="toUser">O ID do utilizador a quem a resposta é enviada.</param>
        /// <param name="reply">A resposta ao pedido (true para aceitar, false para recusar).</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task SendRequestReply(string replier, string toUser, bool reply)
        {
            if (!int.TryParse(replier, out var replierId) || !int.TryParse(toUser, out var toUserId))
            {
                return;
            }

            FriendObjectFromHub replierObject = new()
            {
                FriendId = toUserId
            };

            // encotra os utilizadores da resposta e recetor da resposta
            var to = _context.UtilizadorRegistado
                    .Where(u => u.IDUtilizador == toUserId)
                    .ToArray()[0];

            var replierUser = _context.UtilizadorRegistado
                    .Where(u => u.IDUtilizador == replierId)
                    .ToArray()[0];

            // Encontra o pedido de amizade
            var req = _context.Amizade
                .Where(p => p.DestinatarioFK == replierId && p.RemetenteFK == toUserId)
                .ToArray()[0];

            if (reply)
            {

                req.DataResposta = DateTime.Now;

                // Cria uma nova amizade
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

                // Cria uma nova sala
                SalasChat sala = new()
                {
                    NomeSala = to.NomeUtilizador + "_" + replierUser.NomeUtilizador,
                    SeGrupo = false,
                    NomeFotografia = "default_avatar.png",
                    DataFotografia = DateTime.Now
                };

                // Adiciona a sala, as amizades e os utilizadores
                _context.Add(sala);
                await _context.SaveChangesAsync();

                RegistadosSalasChat toUserRs = new()
                {
                    SalaFK = _context.SalasChat
                        .Where(s => s.NomeSala == sala.NomeSala)
                        .Select(s => s.IDSala)
                        .ToArray()[0],
                    UtilizadorFK = toUserId
                };

                RegistadosSalasChat replierRs = new()
                {
                    SalaFK = _context.SalasChat
                        .Where(s => s.NomeSala == sala.NomeSala)
                        .Select(s => s.IDSala)
                        .ToArray()[0],
                    UtilizadorFK = replierId
                };

                // Salva as alterações
                _context.Update(req);
                _context.Add(friend);
                _context.Add(toUserRs);
                _context.Add(replierRs);
                await _context.SaveChangesAsync();

                // criar objeto de resposta
                var salaRegistadaId = _context.SalasChat
                    .Where(s => s.NomeSala == to.NomeUtilizador + "_" + replierUser.NomeUtilizador)
                    .Select(s => s.IDSala)
                    .ToArray()[0];

                replierObject = new FriendObjectFromHub
                {
                    FriendId = toUserId,
                    CommonRoomId = salaRegistadaId,
                    FriendEmail = to.Email,
                    FriendName = to.NomeUtilizador,
                    ImageURL = $"{Context.GetHttpContext().Request.Scheme}://{Context.GetHttpContext().Request.Host}" + "/imagens/" + to.NomeFotografia
                };

                // Notifica o utilizador recetor sobre a resposta
                await Clients.User(to.AuthenticationID).ReceiveRequestReply(replierObject, reply);

                // Notifica o utilizador remetente sobre a resposta
                await Clients.User(Context.UserIdentifier).ReceiveRequestReply(replierObject, reply);
            }
            else
            {

                // Notifica o utilizador recetor sobre a resposta
                await Clients.User(to.AuthenticationID).ReceiveRequestReply(replierObject, reply);

                // Notifica o utilizador remetente sobre a resposta
                await Clients.User(Context.UserIdentifier).ReceiveRequestReply(replierObject, reply);

                _context.Remove(req);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Expulsa a um utilizador de um grupo
        /// </summary>
        /// <param name="toUser">Utilizador expulso</param>
        /// <param name="roomId">Sala de que 'toUser' foi expulso></param>
        /// <returns></returns>
        public async Task ExpelFromGroup(string toUser, string roomId)
        {

            if (!int.TryParse(toUser, out var toUserId) || !int.TryParse(roomId, out var roomIdInt))
            {
                return;
            }

            var expelledUserQuery = _context.RegistadosSalasChat
                .Where(rs => rs.UtilizadorFK == toUserId && rs.SalaFK == roomIdInt);

            var expelledUser = expelledUserQuery
                .Select(rs => rs.Utilizador)
                .ToArray()[0];

            foreach (var connectionId in _connections.GetConnections(expelledUser.AuthenticationID))
            {
                await Groups.RemoveFromGroupAsync(connectionId, roomId);
            }


            // informar utilizador da sua expulsao
            await Clients.User(expelledUser.AuthenticationID).ReceiveExpelling(roomId, $"Foi expulso da sala {roomId}!");

            // informar grupo da expulsao
            await Clients.Group(roomId).ReceiveExpelling(roomId, $"{toUser} foi expulso da sala!");

            // informar o caller do metodo da expulsao
            await Clients.User(Context.UserIdentifier).ReceiveExpelling(toUser, $"{toUser} foi expulso da sala {roomId}!");

            _context.Remove(expelledUserQuery.ToArray()[0]);
            await _context.SaveChangesAsync();
        }
    }
}