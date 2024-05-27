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
    /// Esta classe implementa um hub em tempo real para gerir a comunicação entre o cliente e o servidor.
    /// </summary>
    [Authorize]
    public class RealTimeHub : Hub<IRealTimeHub>
    {
        /// <summary>
        /// Contexto da base de dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor da classe RealTimeHub.
        /// </summary>
        /// <param name="context">O contexto do banco de dados a ser usado pela classe.</param>
        public RealTimeHub(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método assíncrono que é invocado quando uma conexão é estabelecida.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public override async Task OnConnectedAsync()
        {
            // Obtém o valor do cabeçalho de autorização
            var httpCtx = Context.GetHttpContext();
            string header = httpCtx.Request.Headers.Authorization;
            string token = "";

            if (header != null && header.Length != 0)
            {
                token = this.GenerateTokenFromHeader(header);
            }
            await Clients.Client(Context.ConnectionId).OnConnectedAsyncPrivate("Entraste e o teu ID é o " + Context.ConnectionId);
        }

        /// <summary>
        /// Método assíncrono que é invocado quando uma conexão é desestabelecida.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Context.Abort();
            await Clients.Client(Context.ConnectionId).OnConnectedAsyncPrivate("Saíste!");
        }

        /// <summary>
        /// Gera um token a partir do cabeçalho de autorização.
        /// </summary>
        /// <param name="header">O cabeçalho de autorização.</param>
        /// <returns>O token gerado a partir do cabeçalho.</returns>
        private string GenerateTokenFromHeader(string header)
        {
            string decodedHeader = System.Text.Encoding.UTF8.GetString(Base64.Decode(header));
            return header;
        }

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
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).RemovedFromGroup(Context.ConnectionId, roomId);
        }

        /// <summary>
        /// Envia uma mensagem a um grupo específico.
        /// </summary>
        /// <param name="item">O ID do item associado à mensagem.</param>
        /// <param name="fromUser">O ID do utilizador de origem.</param>
        /// <param name="roomId">O ID do grupo a que a mensagem será enviada.</param>
        /// <param name="message">A mensagem a ser enviada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task SendMessageToRoom(string item, string fromUser, string roomId, string message)
        {
            if (!int.TryParse(item, out var itemId) || !int.TryParse(fromUser, out var fromUserId) || !int.TryParse(roomId, out var roomIdOut))
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
            var messageObject = new ApiModels.Messages()
            {
                IDMensagem = msg.IDMensagem,
                IDRemetente = msg.RemetenteFK,
                URLImagemRemetente = $"{Context.GetHttpContext().Request.Scheme}://{Context.GetHttpContext().Request.Host}" + "/imagens/" + msg.Remetente.NomeFotografia,
                NomeRemetente = msg.Remetente.NomeUtilizador,
                ConteudoMsg = msg.ConteudoMsg
            };

            // Envia a mensagem ao grupo
            await Clients.Group(roomId).ReceiveMessage(itemId, sala.SeGrupo, messageObject);
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
            await Clients.User(toUser).ReceiveBlock(fromUser);
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
            await Clients.User(toUser).ReceiveUnblock(fromUser);
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

            _context.Attach(registados);
            await _context.SaveChangesAsync();

            // Notifica o grupo sobre a entrada do utilizador
            await Clients.Group(group).ReceiveEntry(fromUser + " juntou-se!");
        }

        /// <summary>
        /// Notifica um grupo sobre a saída de um utilizador.
        /// </summary>
        /// <param name="userRemoved">O ID do utilizador que saiu.</param>
        /// <param name="group">O ID do grupo de que o utilizador saiu.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task LeaveGroup(string itemId, string userRemoved, string group)
        {
            if (!int.TryParse(userRemoved, out var userRemovedId) || !int.TryParse(group, out var groupId))
            {
                return;
            }

            // Remove o utilizador do grupo
            RegistadosSalasChat registados = await _context.RegistadosSalasChat
                .FirstOrDefaultAsync(rs => rs.SalaFK == groupId && rs.UtilizadorFK == userRemovedId);

            _context.Remove(registados);
            await _context.SaveChangesAsync();

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);

            // Notifica o grupo sobre a saída do utilizador
            await Clients.Group(group).ReceiveLeaving(itemId, userRemoved + " abandonou o grupo!");

            //Notifica o cliente sobre a sua saída
            await Clients.Client(Context.ConnectionId).ReceiveLeaving(itemId, "Abandonou o grupo " + group);
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

            _context.Attach(req);
            await _context.SaveChangesAsync();

            // Notifica o destinatário sobre o pedido de amizade
            await Clients.User(toUser).ReceiveRequest(fromUser, dest.NomeUtilizador);
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

            if (reply)
            {
                // Recupera os utilizadores que estão a responder e receber a resposta
                var replierUser = _context.UtilizadorRegistado
                    .Where(u => u.IDUtilizador == replierId)
                    .ToArray()[0];

                var to = _context.UtilizadorRegistado
                    .Where(u => u.IDUtilizador == toUserId)
                    .ToArray()[0];

                // Encontra o pedido de amizade
                var req = _context.Amizade
                    .Where(p => p.DestinatarioFK == replierId && p.RemetenteFK == toUserId)
                    .ToArray()[0];

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
                _context.Attach(sala);
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
                _context.Attach(friend);
                _context.Attach(toUserRs);
                _context.Attach(replierRs);
                await _context.SaveChangesAsync();
            }

            // Notifica o utilizador sobre a resposta
            await Clients.User(toUser).ReceiveRequestReply(replier, reply);
        }

        /// <summary>
        /// Expulsa a um utilizador de um grupo
        /// </summary>
        /// <param name="itemId">indice item do utilizador expulso</param>
        /// <param name="toUser">Utilizador expulso</param>
        /// <param name="roomId">Sala de que <see cref="toUser"/> foi expulso></param>
        /// <returns></returns>
        public async Task ExpelFromGroup(string itemId, string toUser, string roomId)
        {

            if(!int.TryParse(itemId, out var itemIdInt) || !int.TryParse(toUser, out var toUserId) || !int.TryParse(roomId, out var roomIdInt))
            {
                return;
            }

            var expelledUser = _context.RegistadosSalasChat
                .Where(rs => rs.UtilizadorFK == toUserId && rs.SalaFK == roomIdInt)
                .ToArray()[0];

            _context.Remove(expelledUser);
            await _context.SaveChangesAsync();

            await Clients.Client(Context.ConnectionId).ReceiveExpelling(itemId);
        }
    }
}