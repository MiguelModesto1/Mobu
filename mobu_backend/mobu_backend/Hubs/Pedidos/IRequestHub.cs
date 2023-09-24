namespace mobu_backend.Hubs.Pedidos
{
    public interface IRequestHub
    {
        Task<string> GetRequestState();

        Task ReceiveRequest(string user, string fromUsername);

        Task ReceiveRequestReply(string user, bool reply);
    }
}
