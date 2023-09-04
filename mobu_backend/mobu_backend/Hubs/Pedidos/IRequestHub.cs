namespace mobu_backend.Hubs.Pedidos
{
    public interface IRequestHub
    {
        Task<int> GetRequestState();

        Task ReceiveRequest(string user, Request request);

        Task ReceiveRequestReply(string user, bool reply);
    }
}
