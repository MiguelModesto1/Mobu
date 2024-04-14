namespace mobu_backend.Hubs.Chat
{
    public interface IRealTimeHub
    {
        Task<string> GetMessage();
        Task ReceiveMessage(int user, string meassage);
        Task OnConnectedAsyncPrivate(string message);
        Task ReceiveBlock(string fromUser);

        Task ReceiveUnblock(string fromUser);
        Task ReceiveEntry(string fromUser);
        Task ReceiveLeaving(string fromUser);
        Task ReceiveRequest(string user, string fromUsername);
        Task ReceiveRequestReply(string user, bool reply);
    }
}
