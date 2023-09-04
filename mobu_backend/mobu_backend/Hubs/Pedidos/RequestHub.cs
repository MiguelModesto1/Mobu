using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using mobu_backend.Data;

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

        public async Task<int> WaitForRequestState(string fromUser)
           => await Clients.User(fromUser).GetRequestState();

        public async Task SendRequestToUser(string fromUser, string toUser, Request request)
            => await Clients.User(toUser).ReceiveRequest(fromUser, request);

        public async Task SendRequestReply(string replier, string toUser, bool reply)
            => await Clients.User(toUser).ReceiveRequestReply(replier, reply);

        /*IMPLEMENTAR METODO DE APAGAR AMIZADES*/
    }
}
