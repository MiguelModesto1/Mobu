namespace mobu_backend.Hubs.Jogo
{
    public interface IGameHub
    {
        Task<string> GetGameRoomState();

        Task ReceiveGameRoomState(string gameRoomState);

        Task ReceiveChallenge(string fromUser, string username);

        Task ReceiveReply(string replier, bool interested);
    }
}
