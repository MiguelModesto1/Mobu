using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;
using mobu_backend.Models;

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

        public async Task AddToRoom(string roomId, string connectionId) 
            => await Groups.AddToGroupAsync(connectionId, roomId);

        public async Task RemoveFromRoom(string roomId, string connectionId) 
            => await Groups.RemoveFromGroupAsync(connectionId, roomId);

        public async Task SendMessageToRoom(string fromUser, string roomId, string message, int messageId)
            {
                // sala
                string salaIdQuery = _context.RegistadosSalasChat
                .Where(rs => rs.SalaFK == int.Parse(roomId) && rs.UtilizadorFK == int.Parse(fromUser))
                .Select(rs => rs.SalaFK).ToString();
                
                int salaId = int.Parse(salaIdQuery);
                
                Mensagem msg = new(){
                    ConteudoMsg = message,
                    IDMensagem = messageId,
                    DataHoraMsg = DateTime.Now,
                    RemetenteFK = int.Parse(fromUser),
                    SalaFK = salaId
                };
                
                await _context.Mensagem.AddAsync(msg);
                _context.Attach(msg);
                await _context.SaveChangesAsync();

                await Clients.Group(roomId).ReceiveMessage(fromUser, message, msg.IDMensagem);
            }
    }
}